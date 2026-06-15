"use client";

import { useState, useEffect, useCallback, type ReactNode } from "react";
import Header from "@/components/layout/Header";
import Footer from "@/components/layout/Footer";
import {
  fetchPublicContents,
  fetchPublicCategories,
  type PublicItem,
  type PubCategory,
} from "@/lib/public";

/* ─── Pagination ─────────────────────────────────────────── */
export function Pagination({
  page, totalPages, setPage,
}: {
  page: number; totalPages: number; setPage: (p: number) => void;
}) {
  if (totalPages <= 1) return null;

  const range = (() => {
    if (totalPages <= 7) return Array.from({ length: totalPages }, (_, i) => i + 1);
    if (page <= 4) return Array.from({ length: 7 }, (_, i) => i + 1);
    if (page >= totalPages - 3) return Array.from({ length: 7 }, (_, i) => totalPages - 6 + i);
    return Array.from({ length: 7 }, (_, i) => page - 3 + i);
  })();

  const btn = (label: ReactNode, active: boolean, disabled: boolean, onClick: () => void) => (
    <button onClick={onClick} disabled={disabled} style={{
      minWidth: 38, height: 38, padding: "0 10px", borderRadius: 10,
      border: `1px solid ${active ? "var(--gold)" : "var(--line)"}`,
      background: active ? "var(--gold)" : "var(--surface)",
      color: disabled ? "var(--muted-2)" : active ? "var(--forest)" : "var(--ink)",
      fontWeight: active ? 700 : 400, cursor: disabled ? "default" : "pointer",
      fontSize: 13, transition: "all .15s",
    }}>
      {label}
    </button>
  );

  return (
    <div style={{ display: "flex", justifyContent: "center", gap: 6, marginTop: 40, flexWrap: "wrap" }}>
      {btn("→", false, page === 1, () => setPage(page - 1))}
      {range.map(p => btn(p, p === page, false, () => setPage(p)))}
      {btn("←", false, page === totalPages, () => setPage(page + 1))}
    </div>
  );
}

/* ─── Skeleton grid ──────────────────────────────────────── */
export function SkeletonGrid({ count, ratio = "56.25%" }: { count: number; ratio?: string }) {
  return (
    <div className="cs-grid">
      {Array.from({ length: count }).map((_, i) => (
        <div key={i} className="animate-pulse" style={{
          borderRadius: 20, overflow: "hidden",
          background: "var(--surface)", border: "1px solid var(--line)",
        }}>
          <div style={{ paddingTop: ratio, background: "var(--surface-2)" }} />
          <div style={{ padding: "14px 16px" }}>
            <div style={{ height: 14, borderRadius: 6, background: "var(--surface-2)", width: "70%", marginBottom: 8 }} />
            <div style={{ height: 11, borderRadius: 6, background: "var(--surface-2)", width: "45%" }} />
          </div>
        </div>
      ))}
    </div>
  );
}

/* ─── Subcategory filter pill ────────────────────────────── */
function SubPill({ label, active, onClick }: { label: string; active: boolean; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  return (
    <button
      onClick={onClick}
      onMouseEnter={() => setHover(true)}
      onMouseLeave={() => setHover(false)}
      style={{
        flexShrink: 0, padding: "7px 20px", borderRadius: 999,
        border: `1px solid ${active ? "var(--gold)" : hover ? "var(--forest)" : "var(--line)"}`,
        background: active ? "var(--gold)" : hover ? "var(--surface-2)" : "transparent",
        color: active ? "var(--forest)" : hover ? "var(--ink)" : "var(--ink-2)",
        fontWeight: active ? 700 : 500, fontSize: 13,
        cursor: "pointer", transition: "all .15s",
        boxShadow: active ? "0 2px 10px rgba(200,168,75,.28)" : "none",
      }}
    >
      {label}
    </button>
  );
}

/* ─── Two-level filter bar ───────────────────────────────── */
/*
 * Level 1 (always): All categories from API
 * Level 2 (when a cat is selected): its subcategories
 * No name-matching required — works regardless of DB category names
 */
function FilterBar({
  allCategories, loadingCats,
  activeCatId, activeSubId,
  onCatSelect, onSubSelect,
}: {
  allCategories: PubCategory[];
  loadingCats: boolean;
  activeCatId: string | null;
  activeSubId: string | null;
  onCatSelect: (id: string | null) => void;
  onSubSelect: (id: string | null) => void;
}) {
  const barStyle: React.CSSProperties = {
    position: "sticky", top: 65, zIndex: 40,
    background: "color-mix(in srgb,var(--bg) 94%,transparent)",
    backdropFilter: "blur(14px)",
    WebkitBackdropFilter: "blur(14px)",
    borderBottom: "1px solid var(--line)",
  };

  if (loadingCats) {
    return (
      <div style={barStyle}>
        <div className="cs-container">
          <div style={{ display: "flex", gap: 8, alignItems: "center", padding: "10px 0" }}>
            <span style={{ fontSize: 12, color: "var(--muted-2)", fontWeight: 600, flexShrink: 0 }}>التصنيف:</span>
            {[90, 110, 80, 130, 75, 100].map((w, i) => (
              <div key={i} className="animate-pulse" style={{ flexShrink: 0, height: 34, width: w, borderRadius: 999, background: "var(--surface-2)" }} />
            ))}
          </div>
        </div>
      </div>
    );
  }

  if (allCategories.length === 0) return null;

  const activeCat = allCategories.find(c => c.id === activeCatId) ?? null;
  const subs = activeCat?.subcategories ?? [];

  return (
    <div style={barStyle}>
      <div className="cs-container">
        {/* Level 1 — all categories */}
        <div style={{ display: "flex", gap: 8, alignItems: "center", padding: "10px 0 8px", overflowX: "auto", scrollbarWidth: "none" }}>
          <span style={{ fontSize: 12, color: "var(--muted-2)", fontWeight: 600, flexShrink: 0 }}>التصنيف:</span>
          <SubPill label="الكل" active={activeCatId === null} onClick={() => onCatSelect(null)} />
          {allCategories.map(cat => (
            <SubPill key={cat.id} label={cat.name} active={cat.id === activeCatId} onClick={() => onCatSelect(cat.id)} />
          ))}
        </div>

        {/* Level 2 — subcategories of selected category */}
        {activeCatId && subs.length > 0 && (
          <div style={{
            display: "flex", gap: 6, alignItems: "center",
            padding: "7px 0 9px", overflowX: "auto", scrollbarWidth: "none",
            borderTop: "1px dashed var(--line)",
            animation: "fadeIn .2s ease",
          }}>
            <span style={{ fontSize: 11, color: "var(--muted-2)", fontWeight: 600, flexShrink: 0 }}>فرعي:</span>
            <SubPill label="الكل" active={activeSubId === null} onClick={() => onSubSelect(null)} />
            {subs.map(sub => (
              <SubPill key={sub.id} label={sub.name} active={sub.id === activeSubId} onClick={() => onSubSelect(sub.id)} />
            ))}
          </div>
        )}
      </div>
      <style>{`@keyframes fadeIn{from{opacity:0;transform:translateY(-4px)}to{opacity:1;transform:none}}`}</style>
    </div>
  );
}

/* ─── Main hook ──────────────────────────────────────────── */
export function useCategoryData(
  page: number,
  catId: string | null,
  subId: string | null,
  pageSize: number,
  mediaType: number,
) {
  const [allCategories, setAllCats] = useState<PubCategory[]>([]);
  const [loadingCats, setLoadingCats] = useState(true);
  const [items, setItems]           = useState<PublicItem[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading]       = useState(true);
  const [error, setError]           = useState(false);

  /* Fetch all categories once */
  useEffect(() => {
    const ctrl = new AbortController();
    fetchPublicCategories(ctrl.signal)
      .then(cats => { setAllCats(cats); setLoadingCats(false); })
      .catch(e => { if (e.name !== "AbortError") setLoadingCats(false); });
    return () => ctrl.abort();
  }, []);

  /* Fetch content: always by mediaType + optional category/subcategory */
  useEffect(() => {
    const ctrl = new AbortController();
    setLoading(true);
    setError(false);
    fetchPublicContents(
      { page, pageSize, mediaType, categoryId: catId ?? undefined, subcategoryId: subId ?? undefined },
      ctrl.signal,
    )
      .then(d => { setItems(d.items); setTotalPages(d.totalPages); })
      .catch(e => { if (e.name !== "AbortError") setError(true); })
      .finally(() => setLoading(false));
    return () => ctrl.abort();
  }, [page, catId, subId, pageSize, mediaType]);

  return { allCategories, loadingCats, items, totalPages, loading, error };
}

/* ─── CategoryScreen ─────────────────────────────────────── */
export interface CategoryScreenProps {
  /** 1=Video 2=Image 3=Audio 4=Document 5=PDF */
  mediaType: number;
  icon: string;
  title: string;
  subtitle: string;
  emptyMessage: string;
  pageSize?: number;
  gridCols?: string;
  skeletonRatio?: string;
  renderCard: (item: PublicItem, onClick: () => void) => ReactNode;
  renderModal?: (item: PublicItem | null, onClose: () => void) => ReactNode;
}

export default function CategoryScreen({
  mediaType, icon, title, subtitle, emptyMessage,
  pageSize = 12, gridCols = "repeat(3,1fr)",
  skeletonRatio = "56.25%",
  renderCard, renderModal,
}: CategoryScreenProps) {
  const [page, setPage]         = useState(1);
  const [catId, setCatId]       = useState<string | null>(null);
  const [subId, setSubId]       = useState<string | null>(null);
  const [activeId, setActiveId] = useState<string | null>(null);

  const { allCategories, loadingCats, items, totalPages, loading, error } = useCategoryData(
    page, catId, subId, pageSize, mediaType,
  );

  const handleCat = useCallback((id: string | null) => {
    setCatId(id);
    setSubId(null);
    setPage(1);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const handleSub = useCallback((id: string | null) => {
    setSubId(id);
    setPage(1);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const handlePage = useCallback((p: number) => {
    setPage(p);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const activeCat = allCategories.find(c => c.id === catId) ?? null;
  const activeSub = activeCat?.subcategories.find(s => s.id === subId) ?? null;
  const activeItem = items.find(i => i.id === activeId) ?? null;

  return (
    <div style={{ minHeight: "100vh", display: "flex", flexDirection: "column", background: "var(--bg)" }}>
      <Header />

      {/* Page header */}
      <div style={{ padding: "28px 0 18px", borderBottom: "1px solid var(--line)" }}>
        <div className="cs-container">
          <div style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", gap: 12, flexWrap: "wrap" }}>
            <div>
              <h1 style={{ fontSize: 26, fontWeight: 800, color: "var(--ink)", fontFamily: "'Noto Kufi Arabic',sans-serif", margin: 0 }}>
                {icon} {title}
              </h1>
              <p style={{ color: "var(--muted)", marginTop: 5, fontSize: 14, display: "flex", alignItems: "center", gap: 6, flexWrap: "wrap" }}>
                {subtitle}
                {activeCat && <><span style={{ color: "var(--line)" }}>›</span><span style={{ color: "var(--forest)", fontWeight: 600 }}>{activeCat.name}</span></>}
                {activeSub && <><span style={{ color: "var(--line)" }}>›</span><span style={{ color: "var(--gold)", fontWeight: 600 }}>{activeSub.name}</span></>}
              </p>
            </div>

            {/* Active filter badges */}
            <div style={{ display: "flex", gap: 6, flexWrap: "wrap" }}>
              {activeSub && (
                <button onClick={() => handleSub(null)} style={{ display: "flex", alignItems: "center", gap: 5, padding: "5px 12px", borderRadius: 999, border: "1px solid var(--gold)", background: "rgba(200,168,75,.08)", color: "var(--gold)", fontSize: 12, fontWeight: 700, cursor: "pointer" }}>
                  {activeSub.name} <span>×</span>
                </button>
              )}
              {activeCat && (
                <button onClick={() => handleCat(null)} style={{ display: "flex", alignItems: "center", gap: 5, padding: "5px 12px", borderRadius: 999, border: "1px solid var(--forest)", background: "rgba(21,128,61,.06)", color: "var(--forest)", fontSize: 12, fontWeight: 700, cursor: "pointer" }}>
                  {activeCat.name} <span>×</span>
                </button>
              )}
            </div>
          </div>
        </div>
      </div>

      {/* Sticky two-level filter bar */}
      <FilterBar
        allCategories={allCategories}
        loadingCats={loadingCats}
        activeCatId={catId}
        activeSubId={subId}
        onCatSelect={handleCat}
        onSubSelect={handleSub}
      />

      {/* Content */}
      <main style={{ flex: 1 }}>
        <div className="cs-container" style={{ padding: "32px 0 56px" }}>

          {error && (
            <div style={{ textAlign: "center", padding: 60 }}>
              <p style={{ color: "var(--muted)", marginBottom: 16 }}>حدث خطأ أثناء التحميل</p>
              <button onClick={() => window.location.reload()} style={{
                padding: "8px 22px", borderRadius: 10, border: "1px solid var(--line)",
                background: "var(--surface)", color: "var(--ink)", cursor: "pointer", fontSize: 13,
              }}>
                إعادة المحاولة
              </button>
            </div>
          )}

          {loading && <SkeletonGrid count={pageSize} ratio={skeletonRatio} />}

          {!loading && !error && items.length === 0 && (
            <div style={{ textAlign: "center", padding: 60 }}>
              <div style={{ fontSize: 48, marginBottom: 16 }}>🔍</div>
              <p style={{ color: "var(--muted)", fontSize: 15 }}>{emptyMessage}</p>
              {(catId || subId) && (
                <button onClick={() => handleCat(null)} style={{
                  marginTop: 16, padding: "8px 22px", borderRadius: 10,
                  border: "1px solid var(--gold)", background: "transparent",
                  color: "var(--forest)", cursor: "pointer", fontSize: 13, fontWeight: 600,
                }}>
                  عرض جميع المحتويات
                </button>
              )}
            </div>
          )}

          {!loading && !error && items.length > 0 && (
            <div className="cs-grid">
              {items.map(item => renderCard(item, () => setActiveId(item.id)))}
            </div>
          )}

          <Pagination page={page} totalPages={totalPages} setPage={handlePage} />
        </div>
      </main>

      <Footer />

      {renderModal?.(activeItem, () => setActiveId(null))}

      <style>{`
        .cs-container { max-width: 1280px; margin: 0 auto; padding-inline: 24px; }
        .cs-grid { display: grid; gap: 24px; grid-template-columns: ${gridCols}; }
        @media (max-width: 1024px) { .cs-grid { grid-template-columns: repeat(2,1fr) !important; } }
        @media (max-width: 640px)  { .cs-grid { grid-template-columns: 1fr !important; } }
      `}</style>
    </div>
  );
}
