"use client";

import { useState, useEffect, useRef, useCallback } from "react";
import {
  fetchPublicCategories, fetchPublicContents, fetchPublicDetail, fetchSignedUrl,
} from "@/lib/public";

const CATEGORY_NAME    = "الاعلانات";
const SUBCATEGORY_NAME = "اسلايدر";
const PAGE_SIZE = 20;

/** Normalize Arabic text: strip diacritics + unify alef variants */
function norm(s: string) {
  return s
    .trim()
    .replace(/[ؐ-ًؚ-ٰٟ]/g, "") // diacritics
    .replace(/[أإآٱ]/g, "ا");                            // alef variants
}

interface Slide {
  id: string;
  title: string;
  url: string;
}

/** Resolve ONE display URL per content item.
 *  Uses the FIRST image asset found; falls back to thumbnailUrl. */
async function resolveSlideUrl(
  item: { id: string; thumbnailUrl: string | null },
  signal: AbortSignal
): Promise<string | null> {
  try {
    const detail = await fetchPublicDetail(item.id, signal);

    // pick the first image asset only
    const firstImage = detail.mediaAssets.find(a =>
      ["Image", "image", "2", "Photo", "photo"].includes(a.mediaType)
    );

    if (firstImage) {
      const { url } = await fetchSignedUrl(firstImage.id, signal);
      return url;
    }

    // no image asset → fall back to thumbnailUrl if set
    return item.thumbnailUrl ?? null;
  } catch (e) {
    console.warn("[Slider] resolveSlideUrl failed:", e);
    // last resort: use thumbnailUrl if available
    return item.thumbnailUrl ?? null;
  }
}

/* ── Single slide image with error fallback ── */
function SlideImage({ slide }: { slide: Slide }) {
  const [err, setErr] = useState(false);
  if (err) {
    return (
      <div style={{
        minWidth: "100%",
        height: "clamp(200px, 32vw, 440px)",
        background: "linear-gradient(135deg,#0b2318,#1a4332)",
        display: "flex", alignItems: "center", justifyContent: "center", flexDirection: "column", gap: 10,
      }}>
        <span style={{ fontSize: 40 }}>🖼️</span>
        <p style={{ color: "rgba(255,255,255,.6)", fontSize: 13 }}>{slide.title}</p>
      </div>
    );
  }
  return (
    <div style={{ minWidth: "100%" }}>
      <img
        src={slide.url}
        alt={slide.title}
        onError={() => { console.warn("[Slider] img load error:", slide.url); setErr(true); }}
        style={{ width: "100%", height: "clamp(200px, 32vw, 440px)", objectFit: "cover", display: "block" }}
      />
    </div>
  );
}

/* ------------------------------------------------------------------ */
/* Slider UI                                                            */
/* ------------------------------------------------------------------ */
export default function HeroBannerSlider() {
  const [slides, setSlides]   = useState<Slide[]>([]);
  const [loading, setLoading] = useState(true);
  const [current, setCurrent] = useState(0);
  const [paused, setPaused]   = useState(false);
  const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

  /* ── fetch slides from API ── */
  useEffect(() => {
    const ctrl = new AbortController();
    (async () => {
      try {
        // 1. find category + subcategory (normalized Arabic matching)
        const cats = await fetchPublicCategories(ctrl.signal);
        console.log("[Slider] categories:", cats.map(c => c.name));

        const cat = cats.find(c => norm(c.name) === norm(CATEGORY_NAME));
        console.log("[Slider] matched category:", cat?.name ?? "NOT FOUND");
        if (!cat) { setLoading(false); return; }

        const sub = cat.subcategories.find(s => norm(s.name) === norm(SUBCATEGORY_NAME));
        console.log("[Slider] matched subcategory:", sub?.name ?? "NOT FOUND",
          "| available:", cat.subcategories.map(s => s.name));
        if (!sub) { setLoading(false); return; }

        // 2. fetch ALL published items in that subcategory (no mediaType filter)
        const page = await fetchPublicContents(
          { categoryId: cat.id, subcategoryId: sub.id, pageSize: PAGE_SIZE },
          ctrl.signal
        );
        console.log("[Slider] items found:", page.items.length, page.items.map(i => i.title));

        // 3. resolve a display URL for every item in parallel
        const resolved = await Promise.all(
          page.items.map(async item => {
            const url = await resolveSlideUrl(item, ctrl.signal);
            console.log("[Slider] item", item.title, "→ url:", url ? "✓" : "null");
            if (!url) return null;
            return { id: item.id, title: item.title, url };
          })
        );

        setSlides(resolved.filter(Boolean) as Slide[]);
      } catch (e: unknown) {
        console.error("[Slider] error:", e);
        if ((e as Error).name !== "AbortError") setSlides([]);
      } finally {
        setLoading(false);
      }
    })();
    return () => ctrl.abort();
  }, []);

  /* ── auto-advance ── */
  const total = slides.length;
  const next  = useCallback(() => setCurrent(c => (c + 1) % total), [total]);
  const prev  = useCallback(() => setCurrent(c => (c - 1 + total) % total), [total]);

  useEffect(() => {
    if (paused || total <= 1) return;
    timerRef.current = setInterval(next, 5000);
    return () => { if (timerRef.current) clearInterval(timerRef.current); };
  }, [paused, next, total]);

  /* ── skeleton while loading ── */
  if (loading) {
    return (
      <div className="animate-pulse" style={{
        borderRadius: 24, overflow: "hidden",
        height: "clamp(200px, 32vw, 440px)",
        background: "var(--surface-2)",
        border: "1px solid var(--line)",
      }} />
    );
  }

  /* ── fallback when no slides ── */
  if (slides.length === 0) return <BannerFallback />;

  /* ── slider ── */
  return (
    <div
      className="relative overflow-hidden"
      style={{ borderRadius: 24, boxShadow: "var(--shadow-lg)", border: "1px solid var(--line-gold)" }}
      onMouseEnter={() => setPaused(true)}
      onMouseLeave={() => setPaused(false)}
    >
      {/* Track */}
      <div style={{
        display: "flex",
        transition: "transform .5s cubic-bezier(.4,0,.2,1)",
        transform: `translateX(${current * 100}%)`,
        direction: "ltr",
      }}>
        {slides.map(slide => (
          <SlideImage key={slide.id} slide={slide} />
        ))}
      </div>

      {/* Arrows */}
      {total > 1 && (
        <>
          <SliderBtn side="right" onClick={prev} label="السابق">›</SliderBtn>
          <SliderBtn side="left"  onClick={next} label="التالي">‹</SliderBtn>
        </>
      )}

      {/* Dots */}
      {total > 1 && (
        <div style={{
          position: "absolute", bottom: 14, left: "50%",
          transform: "translateX(-50%)",
          display: "flex", gap: 7, zIndex: 10,
        }}>
          {slides.map((_, i) => (
            <button
              key={i}
              onClick={() => setCurrent(i)}
              aria-label={`الشريحة ${i + 1}`}
              style={{
                width: i === current ? 24 : 8, height: 8,
                borderRadius: 99, border: "none", cursor: "pointer",
                padding: 0, transition: "all .3s",
                background: i === current ? "var(--gold)" : "rgba(255,255,255,.6)",
                boxShadow: "0 1px 4px rgba(0,0,0,.3)",
              }}
            />
          ))}
        </div>
      )}

      {/* Progress bar */}
      {total > 1 && !paused && (
        <div style={{
          position: "absolute", bottom: 0, left: 0, right: 0,
          height: 3, background: "rgba(0,0,0,.18)",
        }}>
          <div
            key={current}
            style={{
              height: "100%",
              background: "var(--gold)",
              animation: "slideProgress 5s linear forwards",
            }}
          />
        </div>
      )}

      <style>{`
        @keyframes slideProgress { from { width: 0% } to { width: 100% } }
      `}</style>
    </div>
  );
}

/* ── arrow button helper ── */
function SliderBtn({
  side, onClick, label, children,
}: {
  side: "left" | "right";
  onClick: () => void;
  label: string;
  children: React.ReactNode;
}) {
  const [hov, setHov] = useState(false);
  return (
    <button
      onClick={onClick}
      aria-label={label}
      onMouseEnter={() => setHov(true)}
      onMouseLeave={() => setHov(false)}
      style={{
        position: "absolute", top: "50%", [side]: 14,
        transform: "translateY(-50%)",
        width: 40, height: 40, borderRadius: "50%",
        background: hov ? "var(--gold)" : "rgba(255,255,255,.88)",
        border: "none",
        boxShadow: "0 2px 10px rgba(0,0,0,.25)",
        cursor: "pointer", fontSize: 22, lineHeight: 1,
        display: "flex", alignItems: "center", justifyContent: "center",
        zIndex: 10, transition: "background .2s",
        color: hov ? "var(--forest)" : "#1a1a1a",
      }}
    >{children}</button>
  );
}

/* ── CSS fallback banner ── */
function BannerFallback() {
  return (
    <div style={{
      borderRadius: 24,
      border: "1px solid var(--line-gold)",
      boxShadow: "var(--shadow-lg)",
      height: "clamp(200px, 32vw, 440px)",
      background: "linear-gradient(135deg, #0b2318 0%, #1a4332 45%, #0d2a1e 100%)",
      display: "flex", flexDirection: "column",
      alignItems: "center", justifyContent: "center",
      position: "relative", overflow: "hidden",
    }}>
      {/* film strip left */}
      <div style={{
        position: "absolute", left: 0, top: 0, bottom: 0, width: "20%",
        background: "linear-gradient(90deg,rgba(0,0,0,.5),transparent)",
        display: "flex", flexDirection: "column", justifyContent: "center",
        gap: 6, padding: "0 8px",
      }}>
        {Array.from({ length: 6 }).map((_, i) => (
          <div key={i} style={{
            height: 48, borderRadius: 6,
            background: "rgba(255,255,255,.07)",
            border: "1px solid rgba(255,255,255,.1)",
          }} />
        ))}
      </div>
      {/* right shadow */}
      <div style={{
        position: "absolute", right: 0, top: 0, bottom: 0, width: "20%",
        background: "linear-gradient(270deg,rgba(0,0,0,.4),transparent)",
      }} />

      <div style={{ position: "relative", zIndex: 2, textAlign: "center", padding: "0 24px" }}>
        <div style={{
          width: 80, height: 80, borderRadius: "50%", margin: "0 auto 16px",
          background: "rgba(200,168,75,.12)",
          border: "2px solid rgba(200,168,75,.5)",
          display: "flex", alignItems: "center", justifyContent: "center", fontSize: 36,
        }}>🌿</div>
        <h2 style={{
          color: "#fff", fontFamily: "'Noto Kufi Arabic',sans-serif",
          fontWeight: 800, fontSize: "clamp(20px,4vw,38px)", margin: "0 0 10px",
        }}>عالم من المحتوى بين يديك</h2>
        <p style={{ color: "var(--gold)", fontSize: "clamp(13px,2vw,18px)", fontWeight: 600, margin: "0 0 22px" }}>
          فيديوهات &bull; صور &bull; صوت
        </p>
        <div style={{ display: "flex", gap: 24, justifyContent: "center" }}>
          {[{ icon: "🎬", label: "فيديوهات", href: "/video" },
            { icon: "🖼️", label: "صور",       href: "/gallery" },
            { icon: "🎙",  label: "صوت",       href: "/audio"  }].map(item => (
            <a key={item.label} href={item.href} style={{ display: "flex", flexDirection: "column", alignItems: "center", gap: 6, textDecoration: "none" }}>
              <div style={{
                width: 48, height: 48, borderRadius: 12,
                background: "rgba(200,168,75,.15)", border: "1px solid rgba(200,168,75,.3)",
                display: "flex", alignItems: "center", justifyContent: "center", fontSize: 22,
              }}>{item.icon}</div>
              <span style={{ color: "rgba(255,255,255,.8)", fontSize: 12, fontWeight: 600 }}>{item.label}</span>
            </a>
          ))}
        </div>
      </div>
    </div>
  );
}
