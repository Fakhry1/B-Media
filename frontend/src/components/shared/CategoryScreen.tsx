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
    const total = Math.min(totalPages, 7);
    if (totalPages <= 7) return Array.from({ length: total }, (_, i) => i + 1);
    if (page <= 4) return Array.from({ length: 7 }, (_, i) => i + 1);
    if (page >= totalPages - 3) return Array.from({ length: 7 }, (_, i) => totalPages - 6 + i);
    return Array.from({ length: 7 }, (_, i) => page - 3 + i);
  })();

  const btn = (label: ReactNode, active: boolean, disabled: boolean, onClick: () => void) => (
    <button
      onClick={onClick}
      disabled={disabled}
      style={{
        minWidth: 38, height: 38, padding: "0 10px", borderRadius: 10,
        border: `1px solid ${active ? "var(--gold)" : "var(--line)"}`,
        background: active ? "var(--gold)" : "var(--surface)",
        color: disabled ? "var(--muted-2)" : active ? "var(--forest)" : "var(--ink)",
        fontWeight: active ? 700 : 400,
        cursor: disabled ? "default" : "pointer",
        fontSize: 13, transition: "all .15s",
      }}
    >
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

/* ─── SubcategoryTabs ────────────────────────────────────── */
function SubcategoryTabs({
  category, activeId, onSelect,
}: {
  category: PubCategory | null;
  activeId: string | null;
  onSelect: (id: string | null) => void;
}) {
  const subs = category?.subcategories ?? [];
  return (
    <div style={{
      display: "flex", gap: 8, overflowX: "auto",
      padding: "16px 0 0", scrollbarWidth: "none",
    }}>
      {[{ id: null, name: "الكل" }, ...subs].map(sub => {
        const active = sub.id === activeId;
        return (
          <button key={sub.id ?? "all"} onClick={() => onSelect(sub.id)}
            style={{
              flexShrink: 0, padding: "7px 20px", borderRadius: 999, border: "1px solid",
              borderColor: active ? "var(--gold)" : "var(--line)",
              background: active ? "var(--gold)" : "transparent",
              color: active ? "var(--forest)" : "var(--ink)",
              fontWeight: 600, fontSize: 13, cursor: "pointer", transition: "all .15s",
            }}
          >
            {sub.name}
          </button>
        );
      })}
    </div>
  );
}

/* ─── Main hook ──────────────────────────────────────────── */
export function useCategoryData(
  categoryName: string,
  page: number,
  subId: string | null,
  pageSize: number,
  mediaType?: number,
) {
  const [category, setCategory] = useState<PubCategory | null | undefined>(undefined); // undefined = loading
  const [items, setItems] = useState<PublicItem[]>([]);
  const [totalPages, setTotalPages] = useState(1);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  // Find category once (non-blocking)
  useEffect(() => {
    const ctrl = new AbortController();
    fetchPublicCategories(ctrl.signal)
      .then(cats => setCategory(cats.find(c => c.name === categoryName) ?? null))
      .catch(e => { if (e.name !== "AbortError") setCategory(null); });
    return () => ctrl.abort();
  }, [categoryName]);

  // Fetch content — runs as soon as category lookup is done (even if null)
  useEffect(() => {
    if (category === undefined) return; // still loading categories
    const ctrl = new AbortController();
    setLoading(true);
    setError(false);
    fetchPublicContents(
      {
        page,
        pageSize,
        // only pass categoryId when category is found
        categoryId: category?.id,
        subcategoryId: subId ?? undefined,
        // always filter by media type so content shows even without a matching category
        mediaType,
      },
      ctrl.signal,
    )
      .then(d => { setItems(d.items); setTotalPages(d.totalPages); })
      .catch(e => { if (e.name !== "AbortError") setError(true); })
      .finally(() => setLoading(false));
    return () => ctrl.abort();
  }, [category, page, subId, pageSize, mediaType]);

  return { category, items, totalPages, loading, error };
}

/* ─── CategoryScreen component ───────────────────────────── */
export interface CategoryScreenProps {
  /** Category name in DB — used to match and filter by categoryId */
  categoryName: string;
  /**
   * MediaType numeric value from the backend enum:
   * 1=Video, 2=Image, 3=Audio, 4=Document, 5=PDF
   * Always applied so content appears even if the category isn't in the DB yet.
   */
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
  categoryName, mediaType, icon, title, subtitle, emptyMessage,
  pageSize = 12, gridCols = "repeat(3,1fr)",
  skeletonRatio = "56.25%",
  renderCard, renderModal,
}: CategoryScreenProps) {
  const [page, setPage]   = useState(1);
  const [subId, setSubId] = useState<string | null>(null);
  const [activeId, setActiveId] = useState<string | null>(null);

  const { category, items, totalPages, loading, error } = useCategoryData(
    categoryName, page, subId, pageSize, mediaType,
  );

  const handleSub = useCallback((id: string | null) => {
    setSubId(id);
    setPage(1);
  }, []);

  const handlePage = useCallback((p: number) => {
    setPage(p);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const activeItem = items.find(i => i.id === activeId) ?? null;

  return (
    <div style={{ minHeight: "100vh", display: "flex", flexDirection: "column", background: "var(--bg)" }}>
      <Header />

      <main style={{ flex: 1 }}>
        {/* Header bar */}
        <div style={{ padding: "28px 0 0", borderBottom: "1px solid var(--line)" }}>
          <div className="cs-container">
            <h1 style={{
              fontSize: 26, fontWeight: 800, color: "var(--ink)",
              fontFamily: "'Noto Kufi Arabic',sans-serif",
            }}>
              {icon} {title}
            </h1>
            <p style={{ color: "var(--muted)", marginTop: 4, fontSize: 14 }}>{subtitle}</p>
            <SubcategoryTabs category={category} activeId={subId} onSelect={handleSub} />
          </div>
        </div>

        {/* Content */}
        <div className="cs-container" style={{ padding: "32px 0 56px" }}>
          {error && (
            <p style={{ textAlign: "center", padding: 60, color: "var(--muted)" }}>
              حدث خطأ أثناء التحميل — يرجى المحاولة لاحقاً
            </p>
          )}

          {loading && <SkeletonGrid count={pageSize} ratio={skeletonRatio} />}

          {!loading && !error && items.length === 0 && (
            <p style={{ textAlign: "center", padding: 60, color: "var(--muted)" }}>
              {emptyMessage}
            </p>
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
