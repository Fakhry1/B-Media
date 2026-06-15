"use client";

import { useState, useEffect } from "react";
import CategoryScreen from "@/components/shared/CategoryScreen";
import { fetchPublicDetail, fetchSignedUrl, downloadBlob, type PublicItem } from "@/lib/public";

function fmt(iso: string | null) {
  return iso ? new Date(iso).toLocaleDateString("ar-EG", { year: "numeric", month: "long", day: "numeric" }) : "";
}

function VideoCard({ item, onClick }: { item: PublicItem; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  const [dl, setDl] = useState(false);

  async function handleDownload(e: React.MouseEvent) {
    e.stopPropagation();
    setDl(true);
    try {
      const d = await fetchPublicDetail(item.id);
      const a = d.mediaAssets.find(x => x.mediaType.toLowerCase().includes("video"));
      if (!a) return;
      const s = await fetchSignedUrl(a.id);
      await downloadBlob(s.url, item.title + ".mp4");
    } finally { setDl(false); }
  }

  return (
    <div onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{ borderRadius: 20, overflow: "hidden", background: "var(--surface)",
        border: "1px solid var(--line)", boxShadow: hover ? "var(--shadow-md)" : "var(--shadow-sm)",
        transform: hover ? "translateY(-2px)" : "none", transition: "all .2s" }}>
      <div onClick={onClick} style={{ cursor: "pointer" }}>
        <div style={{ position: "relative", paddingTop: "56.25%", overflow: "hidden" }}>
          {item.thumbnailUrl
            ? <img src={item.thumbnailUrl} alt={item.title} loading="lazy"
                style={{ position: "absolute", inset: 0, width: "100%", height: "100%", objectFit: "cover" }} />
            : <div style={{ position: "absolute", inset: 0, background: "linear-gradient(135deg,var(--forest),#0f2d1e)" }} />
          }
          <div style={{ position: "absolute", inset: 0, display: "flex", alignItems: "center", justifyContent: "center",
            background: hover ? "rgba(0,0,0,.38)" : "rgba(0,0,0,.18)", transition: "background .2s" }}>
            <div style={{ width: 52, height: 52, borderRadius: "50%",
              background: hover ? "var(--gold)" : "rgba(255,255,255,.88)",
              display: "flex", alignItems: "center", justifyContent: "center",
              transform: hover ? "scale(1.12)" : "scale(1)", transition: "all .2s" }}>
              <svg width="20" height="20" viewBox="0 0 24 24" fill={hover ? "var(--forest)" : "#1a1a1a"}
                style={{ marginRight: -2 }}><polygon points="5,3 19,12 5,21" /></svg>
            </div>
          </div>
          {item.isFeatured && (
            <div style={{ position: "absolute", top: 10, right: 10, background: "var(--gold)",
              color: "var(--forest)", fontSize: 11, fontWeight: 700, padding: "3px 10px", borderRadius: 20 }}>مميز</div>
          )}
        </div>
        <div style={{ padding: "12px 16px 8px" }}>
          <h3 style={{ color: "var(--ink)", fontSize: 15, fontWeight: 700, lineHeight: 1.4, marginBottom: 5,
            display: "-webkit-box", WebkitLineClamp: 2, WebkitBoxOrient: "vertical", overflow: "hidden" }}>
            {item.title}
          </h3>
          {item.summary && (
            <p style={{ color: "var(--muted)", fontSize: 12, lineHeight: 1.5, margin: 0,
              display: "-webkit-box", WebkitLineClamp: 1, WebkitBoxOrient: "vertical", overflow: "hidden" }}>
              {item.summary}
            </p>
          )}
        </div>
      </div>
      <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between",
        padding: "8px 16px 14px" }}>
        {item.publishedAt && <span style={{ fontSize: 11, color: "var(--muted-2)" }}>{fmt(item.publishedAt)}</span>}
        <button disabled={dl} onClick={handleDownload}
          style={{ display: "flex", alignItems: "center", gap: 5, padding: "5px 12px", borderRadius: 8,
            border: "1px solid var(--line)", background: dl ? "var(--surface-2)" : "transparent",
            color: dl ? "var(--muted-2)" : "var(--forest)", fontSize: 12, fontWeight: 600,
            cursor: dl ? "default" : "pointer", transition: "all .15s" }}>
          {dl ? "⏳" : "⬇"} {dl ? "جارٍ…" : "تحميل"}
        </button>
      </div>
    </div>
  );
}

function VideoModal({ item, onClose }: { item: PublicItem; onClose: () => void }) {
  const [url, setUrl] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [err, setErr] = useState(false);
  const [dl, setDl] = useState(false);

  useEffect(() => {
    const ctrl = new AbortController();
    fetchPublicDetail(item.id, ctrl.signal)
      .then(async d => {
        const a = d.mediaAssets.find(x => x.mediaType.toLowerCase().includes("video"));
        if (!a) { setErr(true); setLoading(false); return; }
        const s = await fetchSignedUrl(a.id, ctrl.signal);
        setUrl(s.url); setLoading(false);
      })
      .catch(e => { if (e.name !== "AbortError") { setErr(true); setLoading(false); } });
    return () => ctrl.abort();
  }, [item.id]);

  return (
    <div onClick={onClose} style={{ position: "fixed", inset: 0, zIndex: 200, background: "rgba(0,0,0,.8)",
      backdropFilter: "blur(8px)", display: "flex", alignItems: "center", justifyContent: "center", padding: 16 }}>
      <div onClick={e => e.stopPropagation()} style={{ background: "var(--surface)", borderRadius: 20,
        width: "100%", maxWidth: 900, maxHeight: "90vh", overflow: "auto", boxShadow: "var(--shadow-lg)" }}>
        <div style={{ display: "flex", alignItems: "flex-start", justifyContent: "space-between",
          padding: "18px 22px 14px", borderBottom: "1px solid var(--line)" }}>
          <div>
            <h2 style={{ color: "var(--ink)", fontWeight: 700, fontSize: 17 }}>{item.title}</h2>
            <div style={{ display: "flex", gap: 10, marginTop: 4 }}>
              {item.categoryName && <span style={{ fontSize: 12, color: "var(--gold)", fontWeight: 600 }}>{item.categoryName}</span>}
              {item.publishedAt && <span style={{ fontSize: 12, color: "var(--muted-2)" }}>{fmt(item.publishedAt)}</span>}
            </div>
          </div>
          <button onClick={onClose} style={{ width: 34, height: 34, borderRadius: 10, border: "1px solid var(--line)",
            background: "var(--surface-2)", cursor: "pointer", fontSize: 18, color: "var(--muted)",
            display: "flex", alignItems: "center", justifyContent: "center" }}>×</button>
        </div>
        <div style={{ padding: 22 }}>
          {loading && (
            <div style={{ paddingTop: "56.25%", position: "relative", background: "var(--surface-2)", borderRadius: 12 }}>
              <div style={{ position: "absolute", inset: 0, display: "flex", flexDirection: "column",
                alignItems: "center", justifyContent: "center", gap: 10 }}>
                <div style={{ width: 38, height: 38, borderRadius: "50%", border: "3px solid var(--gold)",
                  borderTopColor: "transparent", animation: "spin 1s linear infinite" }} />
                <p style={{ color: "var(--muted)", fontSize: 13 }}>جارٍ التحميل…</p>
              </div>
            </div>
          )}
          {err && !loading && <p style={{ textAlign: "center", color: "var(--muted)", padding: "40px 0" }}>تعذّر تحميل الفيديو</p>}
          {url && !loading && (
            <>
              <video controls autoPlay style={{ width: "100%", borderRadius: 12, background: "#000" }} src={url} />
              <div style={{ textAlign: "center", marginTop: 14 }}>
                <button disabled={dl}
                  onClick={async () => { setDl(true); try { await downloadBlob(url, item.title + ".mp4"); } finally { setDl(false); } }}
                  style={{ padding: "8px 22px", borderRadius: 10, border: "none",
                    background: dl ? "var(--line)" : "var(--forest)", color: "#fff",
                    fontSize: 13, fontWeight: 600, cursor: dl ? "default" : "pointer" }}>
                  {dl ? "جارٍ التحميل…" : "⬇ تحميل الفيديو"}
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

export default function VideoPage() {
  return (
    <CategoryScreen
      categoryName="المشاهدة / Video"
      mediaType={1}
      icon="🎬"
      title="المشاهدة"
      subtitle="استعرض جميع مقاطع الفيديو"
      emptyMessage="لا توجد مقاطع فيديو في هذا القسم حالياً"
      pageSize={12}
      renderCard={(item, onClick) => <VideoCard key={item.id} item={item} onClick={onClick} />}
      renderModal={(item, onClose) => item ? <VideoModal item={item} onClose={onClose} /> : null}
    />
  );
}
