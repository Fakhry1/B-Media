"use client";

import { useState, useEffect, useRef } from "react";
import CategoryScreen from "@/components/shared/CategoryScreen";
import { fetchPublicDetail, fetchSignedUrl, downloadBlob, type PublicItem } from "@/lib/public";

function fmt(iso: string | null) {
  return iso ? new Date(iso).toLocaleDateString("ar-EG", { year: "numeric", month: "short", day: "numeric" }) : "";
}

const COLORS = ["#3B82F6","#8B5CF6","#10B981","#F59E0B","#EC4899","#06B6D4","#EF4444"];
const color = (t: string) => COLORS[t.charCodeAt(0) % COLORS.length];

/* ─── Card ── */
function AudioCard({ item, onClick }: { item: PublicItem; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  const c = color(item.title);
  return (
    <div onClick={onClick} onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{ borderRadius: 16, cursor: "pointer", background: "var(--surface)",
        border: "1px solid var(--line)", overflow: "hidden",
        boxShadow: hover ? "var(--shadow-md)" : "var(--shadow-sm)",
        transform: hover ? "translateY(-2px)" : "none", transition: "all .2s",
        display: "flex", alignItems: "center", gap: 16, padding: "16px 18px" }}>
      <div style={{ width: 52, height: 52, borderRadius: "50%", flexShrink: 0,
        background: `linear-gradient(135deg,${c}33,${c}18)`,
        display: "flex", alignItems: "center", justifyContent: "center",
        border: `2px solid ${c}44` }}>
        <svg width="22" height="22" viewBox="0 0 24 24" fill="none" stroke={c} strokeWidth="2">
          <path d="M9 18V5l12-2v13"/><circle cx="6" cy="18" r="3"/><circle cx="18" cy="16" r="3"/>
        </svg>
      </div>
      <div style={{ flex: 1, minWidth: 0 }}>
        <h3 style={{ color: "var(--ink)", fontSize: 14, fontWeight: 700, lineHeight: 1.4, marginBottom: 4,
          overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>{item.title}</h3>
        <div style={{ display: "flex", gap: 10, alignItems: "center" }}>
          {item.categoryName && <span style={{ fontSize: 11, color: c, fontWeight: 600 }}>{item.categoryName}</span>}
          {item.publishedAt && <span style={{ fontSize: 11, color: "var(--muted-2)" }}>{fmt(item.publishedAt)}</span>}
        </div>
      </div>
      <div style={{ width: 34, height: 34, borderRadius: "50%", flexShrink: 0,
        background: hover ? c : "var(--surface-2)",
        display: "flex", alignItems: "center", justifyContent: "center", transition: "background .2s" }}>
        <svg width="14" height="14" viewBox="0 0 24 24" fill={hover ? "#fff" : "var(--muted)"}
          style={{ marginRight: -1 }}><polygon points="5,3 19,12 5,21" /></svg>
      </div>
    </div>
  );
}

/* ─── Modal ── */
function AudioModal({ item, onClose }: { item: PublicItem; onClose: () => void }) {
  const [url, setUrl] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState(false);
  const [dl, setDl] = useState(false);
  const audioRef = useRef<HTMLAudioElement>(null);

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

  const c = color(item.title);

  return (
    <div onClick={onClose} style={{ position: "fixed", inset: 0, zIndex: 200, background: "rgba(0,0,0,.8)",
      backdropFilter: "blur(8px)", display: "flex", alignItems: "center", justifyContent: "center", padding: 16 }}>
      <div onClick={e => e.stopPropagation()} style={{ background: "var(--surface)", borderRadius: 24,
        width: "100%", maxWidth: 520, boxShadow: "var(--shadow-lg)", overflow: "hidden" }}>
        {/* Header strip */}
        <div style={{ height: 6, background: `linear-gradient(90deg,${c},${c}88)` }} />
        <div style={{ padding: "22px 24px" }}>
          <div style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between", marginBottom: 20 }}>
            <div style={{ display: "flex", gap: 14, alignItems: "center" }}>
              <div style={{ width: 56, height: 56, borderRadius: "50%",
                background: `linear-gradient(135deg,${c}33,${c}18)`,
                display: "flex", alignItems: "center", justifyContent: "center", border: `2px solid ${c}44` }}>
                <svg width="24" height="24" viewBox="0 0 24 24" fill="none" stroke={c} strokeWidth="2">
                  <path d="M9 18V5l12-2v13"/><circle cx="6" cy="18" r="3"/><circle cx="18" cy="16" r="3"/>
                </svg>
              </div>
              <div>
                <h2 style={{ color: "var(--ink)", fontWeight: 700, fontSize: 16, lineHeight: 1.3 }}>{item.title}</h2>
                {item.categoryName && <span style={{ fontSize: 12, color: c, fontWeight: 600 }}>{item.categoryName}</span>}
              </div>
            </div>
            <button onClick={onClose} style={{ width: 34, height: 34, borderRadius: 10, border: "1px solid var(--line)",
              background: "var(--surface-2)", cursor: "pointer", fontSize: 18, color: "var(--muted)",
              display: "flex", alignItems: "center", justifyContent: "center", flexShrink: 0 }}>×</button>
          </div>

          {loading && (
            <div style={{ textAlign: "center", padding: "30px 0", display: "flex", flexDirection: "column",
              alignItems: "center", gap: 10 }}>
              <div style={{ width: 36, height: 36, borderRadius: "50%", border: `3px solid ${c}`,
                borderTopColor: "transparent", animation: "spin 1s linear infinite" }} />
              <p style={{ color: "var(--muted)", fontSize: 13 }}>جارٍ التحميل…</p>
            </div>
          )}
          {err && !loading && <p style={{ textAlign: "center", color: "var(--muted)", padding: "20px 0" }}>تعذّر تحميل الملف الصوتي</p>}
          {url && !loading && (
            <>
              <audio ref={audioRef} controls autoPlay style={{ width: "100%", marginBottom: 14 }} src={url} />
              <div style={{ textAlign: "center" }}>
                <button disabled={dl}
                  onClick={async () => { setDl(true); try { await downloadBlob(url, item.title + ".mp3"); } finally { setDl(false); } }}
                  style={{ padding: "8px 22px", borderRadius: 10, border: "none",
                    background: dl ? "var(--line)" : c, color: "#fff",
                    fontSize: 13, fontWeight: 600, cursor: dl ? "default" : "pointer" }}>
                  {dl ? "جارٍ التحميل…" : "⬇ تحميل الملف"}
                </button>
              </div>
            </>
          )}
        </div>
      </div>
      <style>{`@keyframes spin{to{transform:rotate(360deg)}}`}</style>
    </div>
  );
}

export default function AudioPage() {
  return (
    <CategoryScreen
      categoryName="السماع / Audio"
      mediaType={3}
      icon="🎧"
      title="السماع"
      subtitle="استمع إلى المحتوى الصوتي"
      emptyMessage="لا يوجد محتوى صوتي في هذا القسم حالياً"
      pageSize={15}
      gridCols="repeat(2,1fr)"
      skeletonRatio="0%"
      renderCard={(item, onClick) => <AudioCard key={item.id} item={item} onClick={onClick} />}
      renderModal={(item, onClose) => item ? <AudioModal item={item} onClose={onClose} /> : null}
    />
  );
}
