"use client";

import { useState, useEffect } from "react";
import CategoryScreen from "@/components/shared/CategoryScreen";
import { fetchPublicDetail, fetchSignedUrl, downloadBlob, type PublicItem } from "@/lib/public";

function fmt(iso: string | null) {
  return iso ? new Date(iso).toLocaleDateString("ar-EG", { year: "numeric", month: "long", day: "numeric" }) : "";
}

/* ─── Card ── */
function ImageCard({ item, onClick }: { item: PublicItem; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  const COLORS = ["#3B82F6","#8B5CF6","#10B981","#F59E0B","#EC4899","#06B6D4"];
  const c = COLORS[item.title.charCodeAt(0) % COLORS.length];
  return (
    <div onClick={onClick} onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{ borderRadius: 16, overflow: "hidden", cursor: "pointer", background: "var(--surface)",
        border: "1px solid var(--line)",
        boxShadow: hover ? "var(--shadow-md)" : "var(--shadow-sm)",
        transform: hover ? "translateY(-2px) scale(1.01)" : "none", transition: "all .2s" }}>
      <div style={{ position: "relative", paddingTop: "75%", overflow: "hidden" }}>
        {item.thumbnailUrl
          ? <img src={item.thumbnailUrl} alt={item.title} loading="lazy"
              style={{ position: "absolute", inset: 0, width: "100%", height: "100%", objectFit: "cover",
                transform: hover ? "scale(1.06)" : "scale(1)", transition: "transform .3s" }} />
          : <div style={{ position: "absolute", inset: 0, display: "flex", alignItems: "center",
              justifyContent: "center", background: `linear-gradient(135deg,${c}22,${c}44)` }}>
              <svg width="40" height="40" viewBox="0 0 24 24" fill="none" stroke={c} strokeWidth="1.5">
                <rect x="3" y="3" width="18" height="18" rx="2"/>
                <circle cx="8.5" cy="8.5" r="1.5"/>
                <path d="M21 15l-5-5L5 21"/>
              </svg>
            </div>
        }
        {hover && (
          <div style={{ position: "absolute", inset: 0, background: "rgba(0,0,0,.38)",
            display: "flex", alignItems: "center", justifyContent: "center" }}>
            <div style={{ background: "rgba(255,255,255,.9)", borderRadius: "50%", width: 44, height: 44,
              display: "flex", alignItems: "center", justifyContent: "center" }}>
              <svg width="20" height="20" viewBox="0 0 24 24" fill="none" stroke="#1a1a1a" strokeWidth="2">
                <circle cx="11" cy="11" r="8"/><path d="M21 21l-4.35-4.35"/>
                <line x1="11" y1="8" x2="11" y2="14"/><line x1="8" y1="11" x2="14" y2="11"/>
              </svg>
            </div>
          </div>
        )}
        {item.isFeatured && (
          <div style={{ position: "absolute", top: 8, right: 8, background: "var(--gold)",
            color: "var(--forest)", fontSize: 10, fontWeight: 700, padding: "2px 8px", borderRadius: 20 }}>مميز</div>
        )}
      </div>
      <div style={{ padding: "12px 14px" }}>
        <h3 style={{ color: "var(--ink)", fontSize: 13, fontWeight: 700, lineHeight: 1.4, marginBottom: 4,
          display: "-webkit-box", WebkitLineClamp: 2, WebkitBoxOrient: "vertical", overflow: "hidden" }}>
          {item.title}
        </h3>
        <div style={{ display: "flex", gap: 8, alignItems: "center" }}>
          {item.categoryName && <span style={{ fontSize: 11, color: c, fontWeight: 600 }}>{item.categoryName}</span>}
          {item.publishedAt && <span style={{ fontSize: 10, color: "var(--muted-2)" }}>{fmt(item.publishedAt)}</span>}
        </div>
      </div>
    </div>
  );
}

/* ─── Lightbox ── */
function ImageModal({ item, onClose }: { item: PublicItem; onClose: () => void }) {
  const [url, setUrl] = useState<string | null>(item.thumbnailUrl);
  const [loading, setLoading] = useState(true);
  const [dl, setDl] = useState(false);

  useEffect(() => {
    const ctrl = new AbortController();
    fetchPublicDetail(item.id, ctrl.signal)
      .then(async d => {
        const a = d.mediaAssets.find(x => x.mediaType.toLowerCase().includes("image") && x.isPrimary)
          ?? d.mediaAssets.find(x => x.mediaType.toLowerCase().includes("image"));
        if (!a) { setLoading(false); return; }
        const s = await fetchSignedUrl(a.id, ctrl.signal);
        setUrl(s.url); setLoading(false);
      })
      .catch(e => { if (e.name !== "AbortError") setLoading(false); });
    return () => ctrl.abort();
  }, [item.id]);

  return (
    <div onClick={onClose} style={{ position: "fixed", inset: 0, zIndex: 200, background: "rgba(0,0,0,.92)",
      backdropFilter: "blur(12px)", display: "flex", alignItems: "center", justifyContent: "center", padding: 16 }}>
      <button onClick={onClose} style={{ position: "fixed", top: 18, right: 18, width: 38, height: 38,
        borderRadius: "50%", border: "1px solid rgba(255,255,255,.25)", background: "rgba(255,255,255,.1)",
        color: "#fff", fontSize: 20, cursor: "pointer", display: "flex", alignItems: "center", justifyContent: "center" }}>×</button>

      <div onClick={e => e.stopPropagation()} style={{ maxWidth: "90vw", maxHeight: "90vh",
        display: "flex", flexDirection: "column", alignItems: "center", gap: 16 }}>
        {loading && !url && (
          <div style={{ width: 56, height: 56, borderRadius: "50%", border: "3px solid var(--gold)",
            borderTopColor: "transparent", animation: "spin 1s linear infinite" }} />
        )}
        {url && (
          <img src={url} alt={item.title} loading="eager"
            style={{ maxWidth: "90vw", maxHeight: "80vh", objectFit: "contain",
              borderRadius: 12, boxShadow: "0 20px 60px rgba(0,0,0,.6)" }} />
        )}
        <div style={{ textAlign: "center", color: "#fff" }}>
          <p style={{ fontWeight: 700, fontSize: 16, marginBottom: 4 }}>{item.title}</p>
          <div style={{ display: "flex", gap: 16, justifyContent: "center", alignItems: "center" }}>
            {item.publishedAt && <span style={{ fontSize: 12, color: "rgba(255,255,255,.6)" }}>{fmt(item.publishedAt)}</span>}
            {url && (
              <button disabled={dl}
                onClick={async () => { setDl(true); try { await downloadBlob(url, item.title + ".jpg"); } finally { setDl(false); } }}
                style={{ padding: "6px 18px", borderRadius: 8, border: "1px solid rgba(255,255,255,.3)",
                  background: "rgba(255,255,255,.1)", color: "#fff", fontSize: 12, fontWeight: 600,
                  cursor: dl ? "default" : "pointer" }}>
                {dl ? "جارٍ التحميل…" : "⬇ تحميل"}
              </button>
            )}
          </div>
        </div>
      </div>
      <style>{`@keyframes spin{to{transform:rotate(360deg)}}`}</style>
    </div>
  );
}

export default function GalleryPage() {
  return (
    <CategoryScreen
      categoryName="صور / Images"
      mediaType={2}
      icon="🖼️"
      title="صور"
      subtitle="استعرض معرض الصور"
      emptyMessage="لا توجد صور في هذا القسم حالياً"
      pageSize={16}
      gridCols="repeat(4,1fr)"
      skeletonRatio="75%"
      renderCard={(item, onClick) => <ImageCard key={item.id} item={item} onClick={onClick} />}
      renderModal={(item, onClose) => item ? <ImageModal item={item} onClose={onClose} /> : null}
    />
  );
}
