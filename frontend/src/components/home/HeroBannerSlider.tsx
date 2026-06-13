"use client";

import { useState, useEffect, useRef, useCallback } from "react";
import {
  fetchPublicCategories, fetchPublicContents, fetchPublicDetail, fetchSignedUrl,
} from "@/lib/public";

const CATEGORY_NAME    = "الاعلانات";
const SUBCATEGORY_NAME = "اسلايدر";
const PAGE_SIZE        = 20;

/** Normalize Arabic: strip diacritics + unify alef variants */
function norm(s: string) {
  return s.trim()
    .replace(/[ؐ-ًؚ-ٰٟ]/g, "")
    .replace(/[أإآٱ]/g, "ا");
}

interface Slide { id: string; title: string; url: string; }

/** Get signed URL of the FIRST image asset in a content item */
async function getFirstImageUrl(
  item: { id: string; thumbnailUrl: string | null },
  signal: AbortSignal,
): Promise<string | null> {
  try {
    const detail = await fetchPublicDetail(item.id, signal);
    const img = detail.mediaAssets.find(a =>
      ["Image", "image", "2", "Photo", "photo"].includes(a.mediaType)
    );
    if (img) {
      const { url } = await fetchSignedUrl(img.id, signal);
      return url;
    }
    return item.thumbnailUrl ?? null;
  } catch {
    return item.thumbnailUrl ?? null;
  }
}

export default function HeroBannerSlider() {
  const [slides, setSlides]   = useState<Slide[]>([]);
  const [loading, setLoading] = useState(true);
  const [current, setCurrent] = useState(0);
  const [paused, setPaused]   = useState(false);
  const timer = useRef<ReturnType<typeof setInterval> | null>(null);

  useEffect(() => {
    const ctrl = new AbortController();
    (async () => {
      try {
        const cats = await fetchPublicCategories(ctrl.signal);
        const cat  = cats.find(c => norm(c.name) === norm(CATEGORY_NAME));
        if (!cat) { setLoading(false); return; }

        const sub = cat.subcategories.find(s => norm(s.name) === norm(SUBCATEGORY_NAME));
        if (!sub) { setLoading(false); return; }

        const page = await fetchPublicContents(
          { categoryId: cat.id, subcategoryId: sub.id, pageSize: PAGE_SIZE },
          ctrl.signal,
        );

        const resolved = await Promise.all(
          page.items.map(async item => {
            const url = await getFirstImageUrl(item, ctrl.signal);
            return url ? { id: item.id, title: item.title, url } : null;
          })
        );
        setSlides(resolved.filter(Boolean) as Slide[]);
      } catch (e: unknown) {
        if ((e as Error).name !== "AbortError") setSlides([]);
      } finally {
        setLoading(false);
      }
    })();
    return () => ctrl.abort();
  }, []);

  const total = slides.length;
  const next  = useCallback(() => setCurrent(c => (c + 1) % total), [total]);
  const prev  = useCallback(() => setCurrent(c => (c - 1 + total) % total), [total]);

  useEffect(() => {
    if (paused || total <= 1) return;
    timer.current = setInterval(next, 5000);
    return () => { if (timer.current) clearInterval(timer.current); };
  }, [paused, next, total]);

  /* skeleton while loading */
  if (loading) {
    return (
      <div className="animate-pulse" style={{
        borderRadius: 24, height: "clamp(200px, 32vw, 440px)",
        background: "var(--surface-2)", border: "1px solid var(--line)",
      }} />
    );
  }

  /* no images → render nothing */
  if (slides.length === 0) return null;

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
          <div key={slide.id} style={{ minWidth: "100%", flexShrink: 0 }}>
            <img
              src={slide.url}
              alt={slide.title}
              style={{
                width: "100%",
                height: "clamp(200px, 32vw, 440px)",
                objectFit: "cover",
                display: "block",
              }}
            />
          </div>
        ))}
      </div>

      {/* Arrows — only when more than 1 slide */}
      {total > 1 && (
        <>
          <Arrow side="right" label="السابق" onClick={prev}>›</Arrow>
          <Arrow side="left"  label="التالي"  onClick={next}>‹</Arrow>
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
                borderRadius: 99, border: "none", cursor: "pointer", padding: 0,
                transition: "all .3s",
                background: i === current ? "var(--gold)" : "rgba(255,255,255,.6)",
                boxShadow: "0 1px 4px rgba(0,0,0,.3)",
              }}
            />
          ))}
        </div>
      )}

      {/* Progress bar */}
      {total > 1 && !paused && (
        <div style={{ position: "absolute", bottom: 0, left: 0, right: 0, height: 3, background: "rgba(0,0,0,.18)" }}>
          <div key={current} style={{
            height: "100%", background: "var(--gold)",
            animation: "sliderProgress 5s linear forwards",
          }} />
        </div>
      )}

      <style>{`@keyframes sliderProgress { from { width:0 } to { width:100% } }`}</style>
    </div>
  );
}

function Arrow({ side, label, onClick, children }: {
  side: "left" | "right"; label: string; onClick: () => void; children: React.ReactNode;
}) {
  const [hov, setHov] = useState(false);
  return (
    <button
      onClick={onClick} aria-label={label}
      onMouseEnter={() => setHov(true)} onMouseLeave={() => setHov(false)}
      style={{
        position: "absolute", top: "50%", [side]: 14,
        transform: "translateY(-50%)", zIndex: 10,
        width: 40, height: 40, borderRadius: "50%", border: "none",
        background: hov ? "var(--gold)" : "rgba(255,255,255,.88)",
        color: hov ? "var(--forest)" : "#1a1a1a",
        boxShadow: "0 2px 10px rgba(0,0,0,.25)",
        cursor: "pointer", fontSize: 22,
        display: "flex", alignItems: "center", justifyContent: "center",
        transition: "all .2s",
      }}
    >{children}</button>
  );
}
