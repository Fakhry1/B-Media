"use client";

import { useState, useEffect, useRef, useCallback, type MouseEvent, type MouseEventHandler } from "react";
import Header from "@/components/layout/Header";
import Footer from "@/components/layout/Footer";
import { useCategoryData, Pagination } from "@/components/shared/CategoryScreen";
import { fetchPublicDetail, fetchSignedUrl, downloadBlob, type PublicItem, type PubCategory } from "@/lib/public";

const ACCENT = "#8B5CF6";

function fmtDate(iso: string | null) {
  return iso ? new Date(iso).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" }) : "";
}

/* ─── Episode Row (Apple Podcasts style) ─────────────────── */
function EpisodeRow({
  item, rank, onClick,
}: { item: PublicItem; rank: number; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  const [dl, setDl]       = useState(false);

  const PALETTE = ["#8B5CF6","#3B82F6","#10B981","#F59E0B","#EC4899","#06B6D4","#EF4444"];
  const c = PALETTE[item.title.charCodeAt(0) % PALETTE.length];

  async function handleDownload(e: MouseEvent) {
    e.stopPropagation();
    setDl(true);
    try {
      const d = await fetchPublicDetail(item.id);
      const a = d.mediaAssets.find(x => x.mediaType.toLowerCase().includes("audio"));
      if (!a) return;
      const s = await fetchSignedUrl(a.id);
      await downloadBlob(s.url, item.title + ".mp3");
    } finally { setDl(false); }
  }

  return (
    <div
      onMouseEnter={() => setHover(true)}
      onMouseLeave={() => setHover(false)}
      style={{
        display: "flex", alignItems: "center", gap: 14, padding: "10px 12px",
        borderRadius: 12, background: hover ? "var(--surface-2)" : "transparent",
        transition: "background .15s",
      }}
    >
      {/* Rank number */}
      <div style={{
        width: 28, flexShrink: 0, textAlign: "center",
        fontWeight: rank <= 3 ? 800 : 500,
        fontSize: rank <= 3 ? 17 : 14,
        color: rank <= 3 ? c : "var(--muted-2)",
        fontVariantNumeric: "tabular-nums",
      }}>
        {rank}
      </div>

      {/* Artwork */}
      <div style={{
        width: 56, height: 56, borderRadius: 10, flexShrink: 0, overflow: "hidden",
        background: `linear-gradient(135deg,${c}55,${c}22)`,
        display: "flex", alignItems: "center", justifyContent: "center",
        boxShadow: hover ? `0 4px 18px ${c}44` : "0 1px 4px rgba(0,0,0,.1)",
        transition: "box-shadow .2s",
      }}>
        {item.thumbnailUrl
          ? <img src={item.thumbnailUrl} alt={item.title} loading="lazy"
              style={{ width: "100%", height: "100%", objectFit: "cover" }} />
          : <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke={c} strokeWidth="1.8">
              <path d="M9 18V5l12-2v13"/>
              <circle cx="6" cy="18" r="3"/>
              <circle cx="18" cy="16" r="3"/>
            </svg>
        }
      </div>

      {/* Text info */}
      <div style={{ flex: 1, minWidth: 0 }}>
        <div style={{
          color: "var(--ink)", fontSize: 14, fontWeight: 600, lineHeight: 1.35,
          overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap",
          marginBottom: 3,
        }}>
          {item.title}
        </div>
        <div style={{ display: "flex", alignItems: "center", gap: 6, flexWrap: "wrap" }}>
          {item.categoryName && (
            <span style={{ fontSize: 12, color: c, fontWeight: 600 }}>{item.categoryName}</span>
          )}
          {item.categoryName && item.publishedAt && (
            <span style={{ fontSize: 11, color: "var(--muted-2)" }}>·</span>
          )}
          {item.publishedAt && (
            <span style={{ fontSize: 12, color: "var(--muted-2)" }}>{fmtDate(item.publishedAt)}</span>
          )}
        </div>
        {item.summary && (
          <div style={{
            fontSize: 12, color: "var(--muted)", marginTop: 2,
            overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap",
          }}>
            {item.summary}
          </div>
        )}
      </div>

      {/* Actions */}
      <div style={{ display: "flex", alignItems: "center", gap: 8, flexShrink: 0 }}>
        <button disabled={dl} onClick={handleDownload}
          style={{
            display: "flex", alignItems: "center", gap: 5, padding: "5px 13px",
            borderRadius: 20, border: "1px solid var(--line)",
            background: "transparent", color: dl ? "var(--muted-2)" : "var(--muted)",
            fontSize: 12, fontWeight: 500, cursor: dl ? "default" : "pointer", transition: "all .15s",
          }}>
          <svg width="12" height="12" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.2">
            <path d="M12 5v14M5 12l7 7 7-7"/>
          </svg>
          {dl ? "Downloading…" : "Download"}
        </button>

        <button onClick={onClick}
          style={{
            width: 40, height: 40, borderRadius: "50%", border: "none", cursor: "pointer",
            background: hover ? c : "var(--surface-2)",
            display: "flex", alignItems: "center", justifyContent: "center",
            transition: "all .2s",
            boxShadow: hover ? `0 4px 14px ${c}55` : "none",
          }}>
          <svg width="14" height="14" viewBox="0 0 24 24" fill={hover ? "#fff" : "var(--muted)"}
            style={{ marginLeft: 2 }}>
            <polygon points="5,3 19,12 5,21" />
          </svg>
        </button>
      </div>
    </div>
  );
}

/* ─── Skeleton Row ───────────────────────────────────────── */
function SkeletonRow() {
  return (
    <div className="animate-pulse" style={{ display: "flex", alignItems: "center", gap: 14, padding: "10px 12px" }}>
      <div style={{ width: 28, height: 18, borderRadius: 4, background: "var(--surface-2)", flexShrink: 0 }} />
      <div style={{ width: 56, height: 56, borderRadius: 10, background: "var(--surface-2)", flexShrink: 0 }} />
      <div style={{ flex: 1 }}>
        <div style={{ height: 14, borderRadius: 6, background: "var(--surface-2)", width: "65%", marginBottom: 8 }} />
        <div style={{ height: 11, borderRadius: 6, background: "var(--surface-2)", width: "40%" }} />
      </div>
      <div style={{ width: 90, height: 30, borderRadius: 20, background: "var(--surface-2)", flexShrink: 0 }} />
      <div style={{ width: 40, height: 40, borderRadius: "50%", background: "var(--surface-2)", flexShrink: 0 }} />
    </div>
  );
}

/* ─── Subcategory filter pills ───────────────────────────── */
function FilterPills({
  category, activeSubId, onSelect,
}: {
  category: PubCategory | null | undefined;
  activeSubId: string | null;
  onSelect: (id: string | null) => void;
}) {
  const subs = category?.subcategories ?? [];
  if (!category || subs.length === 0) return null;

  return (
    <div style={{
      display: "flex", gap: 8, alignItems: "center", flexWrap: "nowrap",
      overflowX: "auto", scrollbarWidth: "none", paddingBottom: 2,
    }}>
      {[{ id: null, name: "All Episodes" }, ...subs.map(s => ({ id: s.id, name: s.name }))].map(pill => {
        const active = pill.id === activeSubId;
        return (
          <button key={pill.id ?? "all"} onClick={() => onSelect(pill.id)}
            style={{
              flexShrink: 0, padding: "6px 16px", borderRadius: 999, fontSize: 13, fontWeight: active ? 700 : 500,
              border: `1px solid ${active ? ACCENT : "var(--line)"}`,
              background: active ? ACCENT : "transparent",
              color: active ? "#fff" : "var(--ink-2)", cursor: "pointer", transition: "all .15s",
            }}>
            {pill.name}
          </button>
        );
      })}
    </div>
  );
}

/* ─── Audio Player Modal ─────────────────────────────────── */
function AudioModal({ item, onClose }: { item: PublicItem; onClose: () => void }) {
  const [url,     setUrl]     = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [err,     setErr]     = useState(false);
  const [dl,      setDl]      = useState(false);
  const audioRef = useRef<HTMLAudioElement>(null);

  const PALETTE = ["#8B5CF6","#3B82F6","#10B981","#F59E0B","#EC4899","#06B6D4","#EF4444"];
  const c = PALETTE[item.title.charCodeAt(0) % PALETTE.length];

  useEffect(() => {
    const ctrl = new AbortController();
    fetchPublicDetail(item.id, ctrl.signal)
      .then(async d => {
        const a = d.mediaAssets.find(x => x.mediaType.toLowerCase().includes("audio"));
        if (!a) { setErr(true); setLoading(false); return; }
        const s = await fetchSignedUrl(a.id, ctrl.signal);
        setUrl(s.url); setLoading(false);
      })
      .catch(e => { if (e.name !== "AbortError") { setErr(true); setLoading(false); } });
    return () => ctrl.abort();
  }, [item.id]);

  return (
    <div onClick={onClose} style={{
      position: "fixed", inset: 0, zIndex: 200,
      background: "rgba(0,0,0,.75)", backdropFilter: "blur(20px)",
      display: "flex", alignItems: "center", justifyContent: "center", padding: 16,
    }}>
      <div onClick={(e: MouseEvent<HTMLDivElement>) => e.stopPropagation()} style={{
        background: "var(--surface)", borderRadius: 28, width: "100%", maxWidth: 440,
        overflow: "hidden", boxShadow: "0 32px 80px rgba(0,0,0,.4)",
      }}>

        {/* Artwork banner */}
        <div style={{
          height: 220, background: `linear-gradient(160deg,${c}cc,${c}44)`,
          display: "flex", alignItems: "center", justifyContent: "center", position: "relative",
        }}>
          {item.thumbnailUrl
            ? <img src={item.thumbnailUrl} alt={item.title} style={{
                width: 130, height: 130, borderRadius: 20, objectFit: "cover",
                boxShadow: "0 16px 48px rgba(0,0,0,.5)",
              }} />
            : <div style={{
                width: 130, height: 130, borderRadius: 20,
                background: "rgba(255,255,255,.15)",
                display: "flex", alignItems: "center", justifyContent: "center",
                boxShadow: "0 16px 48px rgba(0,0,0,.4)",
              }}>
                <svg width="48" height="48" viewBox="0 0 24 24" fill="none" stroke="rgba(255,255,255,.8)" strokeWidth="1.5">
                  <path d="M9 18V5l12-2v13"/>
                  <circle cx="6" cy="18" r="3"/>
                  <circle cx="18" cy="16" r="3"/>
                </svg>
              </div>
          }
          <button onClick={onClose} style={{
            position: "absolute", top: 14, right: 14,
            width: 32, height: 32, borderRadius: "50%",
            background: "rgba(0,0,0,.35)", border: "none",
            color: "#fff", fontSize: 18, cursor: "pointer",
            display: "flex", alignItems: "center", justifyContent: "center",
          }}>×</button>
        </div>

        {/* Info + player */}
        <div style={{ padding: "20px 24px 26px" }}>
          <div style={{ marginBottom: 18 }}>
            <h2 style={{ color: "var(--ink)", fontWeight: 700, fontSize: 17, lineHeight: 1.35,
              marginBottom: 5, margin: 0 }}>
              {item.title}
            </h2>
            <div style={{ display: "flex", alignItems: "center", gap: 8, marginTop: 6 }}>
              {item.categoryName && (
                <span style={{ fontSize: 13, color: c, fontWeight: 600 }}>{item.categoryName}</span>
              )}
              {item.publishedAt && (
                <span style={{ fontSize: 12, color: "var(--muted-2)" }}>{fmtDate(item.publishedAt)}</span>
              )}
            </div>
            {item.summary && (
              <p style={{ color: "var(--muted)", fontSize: 13, lineHeight: 1.6, marginTop: 10, marginBottom: 0 }}>
                {item.summary}
              </p>
            )}
          </div>

          {loading && (
            <div style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 10, padding: "16px 0" }}>
              <div style={{ width: 32, height: 32, borderRadius: "50%", border: `3px solid ${c}`,
                borderTopColor: "transparent", animation: "spin 1s linear infinite" }} />
              <p style={{ color: "var(--muted)", fontSize: 13, margin: 0 }}>Loading audio…</p>
            </div>
          )}

          {err && !loading && (
            <p style={{ color: "var(--muted)", textAlign: "center", fontSize: 14, padding: "12px 0" }}>
              Failed to load audio file
            </p>
          )}

          {url && !loading && (
            <>
              <audio ref={audioRef} controls autoPlay src={url}
                style={{ width: "100%", marginBottom: 14, borderRadius: 8, accentColor: c }} />
              <button disabled={dl}
                onClick={async () => { setDl(true); try { await downloadBlob(url, item.title + ".mp3"); } finally { setDl(false); } }}
                style={{
                  width: "100%", padding: "11px 0", borderRadius: 14, border: "none",
                  background: dl ? "var(--line)" : c, color: "#fff",
                  fontSize: 14, fontWeight: 700, cursor: dl ? "default" : "pointer",
                  display: "flex", alignItems: "center", justifyContent: "center", gap: 8,
                }}>
                <svg width="15" height="15" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
                  <path d="M12 5v14M5 12l7 7 7-7"/>
                </svg>
                {dl ? "Downloading…" : "Download Episode"}
              </button>
            </>
          )}
        </div>
      </div>
      <style>{`@keyframes spin{to{transform:rotate(360deg)}}`}</style>
    </div>
  );
}

/* ─── Page ───────────────────────────────────────────────── */
const PAGE_SIZE = 15;

export default function AudioPage() {
  const [page,      setPage]      = useState(1);
  const [subId,     setSubId]     = useState<string | null>(null);
  const [activeId,  setActiveId]  = useState<string | null>(null);

  const { category, items, totalPages, loading, error } = useCategoryData(
    "السماع / Audio", page, subId, PAGE_SIZE, 3,
  );

  const handleSub = useCallback((id: string | null) => {
    setSubId(id);
    setPage(1);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const handlePage = useCallback((p: number) => {
    setPage(p);
    window.scrollTo({ top: 0, behavior: "smooth" });
  }, []);

  const activeItem = items.find((i: PublicItem) => i.id === activeId) ?? null;

  return (
    <div style={{ minHeight: "100vh", display: "flex", flexDirection: "column", background: "var(--bg)" }}>
      <Header />

      {/* ── Hero banner ── */}
      <div style={{
        background: `linear-gradient(150deg,#4c1d95 0%,#5b21b6 40%,#2e1065 100%)`,
        padding: "48px 0 40px", position: "relative", overflow: "hidden",
      }}>
        {/* Dot texture */}
        <div style={{ position: "absolute", inset: 0, opacity: .06,
          backgroundImage: "radial-gradient(circle,#fff 1px,transparent 1px)",
          backgroundSize: "32px 32px" }} />
        {/* Gold stripe */}
        <div style={{ position: "absolute", top: 0, left: 0, right: 0, height: 4,
          background: "linear-gradient(90deg,var(--gold),#e8c05a,var(--gold))" }} />

        <div className="au-wrap" style={{ position: "relative", zIndex: 1 }}>
          <div style={{ display: "flex", alignItems: "center", gap: 20, flexWrap: "wrap" }}>
            {/* Icon */}
            <div style={{
              width: 72, height: 72, borderRadius: 18,
              background: "rgba(255,255,255,.1)", border: "1px solid rgba(255,255,255,.2)",
              display: "flex", alignItems: "center", justifyContent: "center", flexShrink: 0,
            }}>
              <svg width="36" height="36" viewBox="0 0 24 24" fill="none" stroke="rgba(255,255,255,.9)" strokeWidth="1.5">
                <path d="M9 18V5l12-2v13"/>
                <circle cx="6" cy="18" r="3"/>
                <circle cx="18" cy="16" r="3"/>
              </svg>
            </div>

            <div>
              <div style={{ fontSize: 11, fontWeight: 700, color: "rgba(255,255,255,.5)",
                letterSpacing: "1.5px", textTransform: "uppercase", marginBottom: 5 }}>
                B-Media Audio
              </div>
              <h1 style={{ color: "#fff", margin: 0, fontSize: "clamp(24px,4vw,38px)",
                fontWeight: 900, lineHeight: 1.1, fontFamily: "'Noto Kufi Arabic',sans-serif" }}>
                Top Episodes
              </h1>
              <p style={{ color: "rgba(255,255,255,.55)", margin: "6px 0 0", fontSize: 14 }}>
                السماع · اختر ما تريد الاستماع إليه
              </p>
            </div>
          </div>
        </div>
      </div>

      {/* ── Sticky filter bar ── */}
      {category && (category.subcategories?.length ?? 0) > 0 && (
        <div style={{
          position: "sticky", top: 65, zIndex: 40,
          background: "color-mix(in srgb,var(--bg) 92%,transparent)",
          backdropFilter: "blur(14px)", WebkitBackdropFilter: "blur(14px)",
          borderBottom: "1px solid var(--line)", padding: "10px 0",
        }}>
          <div className="au-wrap">
            <FilterPills category={category} activeSubId={subId} onSelect={handleSub} />
          </div>
        </div>
      )}

      {/* ── Episodes list ── */}
      <main style={{ flex: 1 }}>
        <div className="au-wrap" style={{ padding: "32px 0 56px" }}>

          {/* Section header */}
          <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between",
            marginBottom: 20, paddingBottom: 14, borderBottom: "2px solid var(--line)" }}>
            <div style={{ display: "flex", alignItems: "center", gap: 10 }}>
              <div style={{ width: 4, height: 22, borderRadius: 2, background: ACCENT }} />
              <span style={{ fontWeight: 800, fontSize: 18, color: "var(--ink)" }}>Top Episodes</span>
            </div>
            {!loading && items.length > 0 && (
              <span style={{ fontSize: 13, color: "var(--muted-2)" }}>
                {items.length} of {totalPages * PAGE_SIZE}+ episodes
              </span>
            )}
          </div>

          {/* Error */}
          {error && (
            <div style={{ textAlign: "center", padding: 60 }}>
              <p style={{ color: "var(--muted)", marginBottom: 16 }}>Failed to load episodes</p>
              <button onClick={() => window.location.reload()} style={{
                padding: "8px 22px", borderRadius: 10, border: "1px solid var(--line)",
                background: "var(--surface)", color: "var(--ink)", cursor: "pointer", fontSize: 13,
              }}>Try Again</button>
            </div>
          )}

          {/* List */}
          <div style={{ background: "var(--surface)", borderRadius: 16,
            border: "1px solid var(--line)", overflow: "hidden",
            boxShadow: "0 1px 6px rgba(0,0,0,.06)" }}>
            {loading
              ? Array.from({ length: PAGE_SIZE }).map((_, i) => (
                  <div key={i}>
                    {i > 0 && <div style={{ height: 1, background: "var(--line)", margin: "0 12px" }} />}
                    <SkeletonRow />
                  </div>
                ))
              : !error && items.length === 0
                ? <div style={{ textAlign: "center", padding: 60 }}>
                    <div style={{ fontSize: 40, marginBottom: 14 }}>🎧</div>
                    <p style={{ color: "var(--muted)", fontSize: 15 }}>No episodes available yet</p>
                  </div>
                : items.map((item: PublicItem, i: number) => (
                    <div key={item.id}>
                      {i > 0 && <div style={{ height: 1, background: "var(--line)", margin: "0 12px" }} />}
                      <EpisodeRow
                        item={item}
                        rank={(page - 1) * PAGE_SIZE + i + 1}
                        onClick={() => setActiveId(item.id)}
                      />
                    </div>
                  ))
            }
          </div>

          <Pagination page={page} totalPages={totalPages} setPage={handlePage} />
        </div>
      </main>

      <Footer />

      {activeItem && <AudioModal item={activeItem} onClose={() => setActiveId(null)} />}

      <style>{`
        .au-wrap { max-width: 860px; margin: 0 auto; padding-inline: 24px; }
        @media (max-width: 480px) { .au-wrap { padding-inline: 14px; } }
      `}</style>
    </div>
  );
}
