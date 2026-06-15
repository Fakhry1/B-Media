"use client";

import { useState } from "react";
import CategoryScreen from "@/components/shared/CategoryScreen";
import { fetchPublicDetail, fetchSignedUrl, downloadBlob, type PublicItem } from "@/lib/public";

function fmt(iso: string | null) {
  return iso ? new Date(iso).toLocaleDateString("ar-EG", { year: "numeric", month: "long", day: "numeric" }) : "";
}

const COLORS = ["#3B82F6","#8B5CF6","#10B981","#F59E0B","#EC4899","#06B6D4","#EF4444"];
const color = (t: string) => COLORS[t.charCodeAt(0) % COLORS.length];

/* ─── Card ── */
function ReadCard({ item, onClick }: { item: PublicItem; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  const c = color(item.title);
  return (
    <div onClick={onClick} onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{ borderRadius: 20, overflow: "hidden", cursor: "pointer", background: "var(--surface)",
        border: "1px solid var(--line)", display: "flex", flexDirection: "column",
        boxShadow: hover ? "var(--shadow-md)" : "var(--shadow-sm)",
        transform: hover ? "translateY(-2px)" : "none", transition: "all .2s" }}>
      <div style={{ height: 5, background: `linear-gradient(90deg,${c},${c}66)` }} />
      <div style={{ padding: "16px 18px", flex: 1, display: "flex", flexDirection: "column", gap: 8 }}>
        <div style={{ display: "flex", alignItems: "center", gap: 10 }}>
          <div style={{ width: 34, height: 34, borderRadius: 10, flexShrink: 0,
            background: `${c}18`, display: "flex", alignItems: "center", justifyContent: "center" }}>
            <svg width="18" height="18" viewBox="0 0 24 24" fill="none" stroke={c} strokeWidth="2">
              <path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/>
              <polyline points="14 2 14 8 20 8"/>
              <line x1="16" y1="13" x2="8" y2="13"/><line x1="16" y1="17" x2="8" y2="17"/>
              <polyline points="10 9 9 9 8 9"/>
            </svg>
          </div>
          {item.categoryName && (
            <span style={{ fontSize: 11, fontWeight: 700, padding: "2px 8px", borderRadius: 20,
              background: `${c}18`, color: c }}>{item.categoryName}</span>
          )}
          {item.isFeatured && (
            <span style={{ fontSize: 10, fontWeight: 700, padding: "2px 8px", borderRadius: 20,
              background: "var(--gold)", color: "var(--forest)" }}>مميز</span>
          )}
        </div>
        <h3 style={{ color: "var(--ink)", fontSize: 15, fontWeight: 700, lineHeight: 1.45, margin: 0,
          display: "-webkit-box", WebkitLineClamp: 2, WebkitBoxOrient: "vertical", overflow: "hidden" }}>
          {item.title}
        </h3>
        {item.summary && (
          <p style={{ color: "var(--muted)", fontSize: 13, lineHeight: 1.6, margin: 0,
            display: "-webkit-box", WebkitLineClamp: 2, WebkitBoxOrient: "vertical", overflow: "hidden" }}>
            {item.summary}
          </p>
        )}
        <div style={{ display: "flex", gap: 8, alignItems: "center", marginTop: "auto", paddingTop: 4 }}>
          {item.publishedAt && <span style={{ fontSize: 11, color: "var(--muted-2)" }}>{fmt(item.publishedAt)}</span>}
        </div>
      </div>
    </div>
  );
}

/* ─── Modal ── */
function ReadModal({ item, onClose }: { item: PublicItem; onClose: () => void }) {
  const [url, setUrl] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState(false);
  const [dl, setDl] = useState(false);
  const c = color(item.title);

  useState(() => {
    const ctrl = new AbortController();
    fetchPublicDetail(item.id, ctrl.signal)
      .then(async d => {
        const a = d.mediaAssets.find(x => ["pdf","document"].some(t => x.mediaType.toLowerCase().includes(t)));
        if (!a) { setErr(true); setLoading(false); return; }
        const s = await fetchSignedUrl(a.id, ctrl.signal);
        setUrl(s.url); setLoading(false);
      })
      .catch(e => { if (e.name !== "AbortError") { setErr(true); setLoading(false); } });
    return () => ctrl.abort();
  });

  return (
    <div onClick={onClose} style={{ position: "fixed", inset: 0, zIndex: 200, background: "rgba(0,0,0,.8)",
      backdropFilter: "blur(8px)", display: "flex", alignItems: "center", justifyContent: "center", padding: 16 }}>
      <div onClick={e => e.stopPropagation()} style={{ background: "var(--surface)", borderRadius: 20,
        width: "100%", maxWidth: 600, maxHeight: "90vh", overflow: "auto", boxShadow: "var(--shadow-lg)" }}>
        <div style={{ height: 5, background: `linear-gradient(90deg,${c},${c}66)` }} />
        <div style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between",
          padding: "18px 22px 14px", borderBottom: "1px solid var(--line)" }}>
          <div>
            <h2 style={{ color: "var(--ink)", fontWeight: 700, fontSize: 17, lineHeight: 1.3 }}>{item.title}</h2>
            <div style={{ display: "flex", gap: 10, marginTop: 6 }}>
              {item.categoryName && <span style={{ fontSize: 12, color: c, fontWeight: 600 }}>{item.categoryName}</span>}
              {item.publishedAt && <span style={{ fontSize: 12, color: "var(--muted-2)" }}>{fmt(item.publishedAt)}</span>}
            </div>
          </div>
          <button onClick={onClose} style={{ width: 34, height: 34, borderRadius: 10, border: "1px solid var(--line)",
            background: "var(--surface-2)", cursor: "pointer", fontSize: 18, color: "var(--muted)",
            display: "flex", alignItems: "center", justifyContent: "center", flexShrink: 0 }}>×</button>
        </div>
        <div style={{ padding: "22px 24px" }}>
          {item.summary && <p style={{ color: "var(--muted)", fontSize: 14, lineHeight: 1.7, marginBottom: 20 }}>{item.summary}</p>}
          {loading && (
            <div style={{ textAlign: "center", padding: "30px 0", display: "flex", flexDirection: "column",
              alignItems: "center", gap: 10 }}>
              <div style={{ width: 36, height: 36, borderRadius: "50%", border: `3px solid ${c}`,
                borderTopColor: "transparent", animation: "spin 1s linear infinite" }} />
              <p style={{ color: "var(--muted)", fontSize: 13 }}>جارٍ التحميل…</p>
            </div>
          )}
          {err && !loading && (
            <div style={{ textAlign: "center", padding: "20px 0" }}>
              <p style={{ color: "var(--muted)", marginBottom: 12 }}>تعذّر تحميل الملف</p>
            </div>
          )}
          {url && !loading && (
            <div style={{ display: "flex", flexDirection: "column", gap: 12 }}>
              <iframe src={url} style={{ width: "100%", height: 480, borderRadius: 12, border: "1px solid var(--line)" }} />
              <div style={{ textAlign: "center" }}>
                <button disabled={dl}
                  onClick={async () => { setDl(true); try { await downloadBlob(url, item.title + ".pdf"); } finally { setDl(false); } }}
                  style={{ padding: "8px 22px", borderRadius: 10, border: "none",
                    background: dl ? "var(--line)" : c, color: "#fff",
                    fontSize: 13, fontWeight: 600, cursor: dl ? "default" : "pointer" }}>
                  {dl ? "جارٍ التحميل…" : "⬇ تحميل الملف"}
                </button>
              </div>
            </div>
          )}
        </div>
      </div>
      <style>{`@keyframes spin{to{transform:rotate(360deg)}}`}</style>
    </div>
  );
}

export default function ArticlesPage() {
  return (
    <CategoryScreen
      categoryName="الاطلاع / Reading"
      mediaType={5}
      icon="📖"
      title="الاطلاع"
      subtitle="تصفّح المقالات والمستندات"
      emptyMessage="لا توجد مقالات في هذا القسم حالياً"
      pageSize={12}
      gridCols="repeat(3,1fr)"
      skeletonRatio="40%"
      renderCard={(item, onClick) => <ReadCard key={item.id} item={item} onClick={onClick} />}
      renderModal={(item, onClose) => item ? <ReadModal item={item} onClose={onClose} /> : null}
    />
  );
}
