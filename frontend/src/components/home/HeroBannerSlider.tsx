"use client";

import { useState, useEffect, useRef, useCallback } from "react";

const slides = [
  {
    id: "banner",
    type: "image" as const,
    src: "/images/hero-banner.jpg",
    alt: "عالم من المحتوى بين يديك",
  },
  // أضف شرائح إضافية هنا بنفس الهيكل
];

export default function HeroBannerSlider() {
  const [current, setCurrent] = useState(0);
  const [paused, setPaused] = useState(false);
  const [imgError, setImgError] = useState<Record<number, boolean>>({});
  const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);
  const total = slides.length;

  const next = useCallback(() => setCurrent(c => (c + 1) % total), [total]);
  const prev = useCallback(() => setCurrent(c => (c - 1 + total) % total), [total]);

  useEffect(() => {
    if (paused || total <= 1) return;
    timerRef.current = setInterval(next, 5000);
    return () => { if (timerRef.current) clearInterval(timerRef.current); };
  }, [paused, next, total]);

  return (
    <div
      className="relative overflow-hidden"
      style={{ borderRadius: 24, boxShadow: "var(--shadow-lg)", border: "1px solid var(--line-gold)" }}
      onMouseEnter={() => setPaused(true)}
      onMouseLeave={() => setPaused(false)}
    >
      {/* Slides */}
      <div
        style={{
          display: "flex",
          transition: "transform .5s cubic-bezier(.4,0,.2,1)",
          transform: `translateX(${current * 100}%)`,
          direction: "ltr",
        }}
      >
        {slides.map((slide, i) => (
          <div key={slide.id} style={{ minWidth: "100%", position: "relative" }}>
            {!imgError[i] ? (
              <img
                src={slide.src}
                alt={slide.alt}
                onError={() => setImgError(p => ({ ...p, [i]: true }))}
                style={{
                  width: "100%",
                  height: "clamp(260px, 38vw, 480px)",
                  objectFit: "cover",
                  display: "block",
                }}
              />
            ) : (
              /* Fallback: CSS recreation of the banner */
              <BannerFallback />
            )}
          </div>
        ))}
      </div>

      {/* Prev / Next arrows */}
      {total > 1 && (
        <>
          <button
            onClick={prev}
            aria-label="السابق"
            style={{
              position: "absolute", top: "50%", right: 14,
              transform: "translateY(-50%)",
              width: 40, height: 40, borderRadius: "50%",
              background: "rgba(255,255,255,.88)", border: "none",
              boxShadow: "0 2px 10px rgba(0,0,0,.2)",
              cursor: "pointer", fontSize: 18, display: "flex",
              alignItems: "center", justifyContent: "center",
              zIndex: 10, transition: "background .2s",
            }}
            onMouseEnter={e => ((e.currentTarget as HTMLElement).style.background = "var(--gold)")}
            onMouseLeave={e => ((e.currentTarget as HTMLElement).style.background = "rgba(255,255,255,.88)")}
          >›</button>
          <button
            onClick={next}
            aria-label="التالي"
            style={{
              position: "absolute", top: "50%", left: 14,
              transform: "translateY(-50%)",
              width: 40, height: 40, borderRadius: "50%",
              background: "rgba(255,255,255,.88)", border: "none",
              boxShadow: "0 2px 10px rgba(0,0,0,.2)",
              cursor: "pointer", fontSize: 18, display: "flex",
              alignItems: "center", justifyContent: "center",
              zIndex: 10, transition: "background .2s",
            }}
            onMouseEnter={e => ((e.currentTarget as HTMLElement).style.background = "var(--gold)")}
            onMouseLeave={e => ((e.currentTarget as HTMLElement).style.background = "rgba(255,255,255,.88)")}
          >‹</button>
        </>
      )}

      {/* Dots */}
      {total > 1 && (
        <div style={{
          position: "absolute", bottom: 12, left: "50%",
          transform: "translateX(-50%)",
          display: "flex", gap: 7, zIndex: 10,
        }}>
          {slides.map((_, i) => (
            <button
              key={i}
              onClick={() => setCurrent(i)}
              aria-label={`الشريحة ${i + 1}`}
              style={{
                width: i === current ? 22 : 8,
                height: 8, borderRadius: 99,
                background: i === current ? "var(--gold)" : "rgba(255,255,255,.55)",
                border: "none", cursor: "pointer",
                transition: "all .3s",
                padding: 0,
              }}
            />
          ))}
        </div>
      )}

      {/* Progress bar */}
      {total > 1 && !paused && (
        <div style={{
          position: "absolute", bottom: 0, left: 0, right: 0,
          height: 3, background: "rgba(255,255,255,.2)",
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
        @keyframes slideProgress {
          from { width: 0% }
          to   { width: 100% }
        }
      `}</style>
    </div>
  );
}

/* CSS recreation of the banner for when the image file is missing */
function BannerFallback() {
  return (
    <div style={{
      width: "100%",
      height: "clamp(260px, 38vw, 480px)",
      background: "linear-gradient(135deg, #0b2318 0%, #1a4332 40%, #0d2a1e 100%)",
      display: "flex",
      flexDirection: "column",
      alignItems: "center",
      justifyContent: "center",
      position: "relative",
      overflow: "hidden",
    }}>
      {/* Decorative film strip left */}
      <div style={{
        position: "absolute", left: 0, top: 0, bottom: 0, width: "22%",
        background: "linear-gradient(90deg, rgba(0,0,0,.55) 0%, transparent 100%)",
        display: "flex", flexDirection: "column", justifyContent: "center",
        gap: 6, padding: "0 8px",
      }}>
        {Array.from({ length: 6 }).map((_, i) => (
          <div key={i} style={{
            height: 52, borderRadius: 6,
            background: "rgba(255,255,255,.08)",
            border: "1px solid rgba(255,255,255,.12)",
          }} />
        ))}
      </div>

      {/* Decorative right side */}
      <div style={{
        position: "absolute", right: 0, top: 0, bottom: 0, width: "22%",
        background: "linear-gradient(270deg, rgba(0,0,0,.45) 0%, transparent 100%)",
      }} />

      {/* Center content */}
      <div style={{ position: "relative", zIndex: 2, textAlign: "center", padding: "0 24px" }}>
        {/* Logo geometric */}
        <div style={{
          width: 88, height: 88, borderRadius: "50%", margin: "0 auto 18px",
          background: "linear-gradient(135deg, #1a4332, #0b2318)",
          border: "3px solid rgba(200,168,75,.6)",
          boxShadow: "0 0 30px rgba(200,168,75,.25), 0 0 0 6px rgba(200,168,75,.1)",
          display: "flex", alignItems: "center", justifyContent: "center",
          fontSize: 38,
        }}>🌿</div>

        <h2 style={{
          color: "#fff",
          fontFamily: "'Noto Kufi Arabic', sans-serif",
          fontWeight: 800,
          fontSize: "clamp(20px, 4vw, 38px)",
          margin: "0 0 10px",
          textShadow: "0 2px 12px rgba(0,0,0,.4)",
        }}>
          عالم من المحتوى بين يديك
        </h2>

        <p style={{
          color: "var(--gold)",
          fontSize: "clamp(13px, 2vw, 18px)",
          fontWeight: 600,
          letterSpacing: "0.05em",
          margin: "0 0 24px",
        }}>
          فيديوهات &bull; صور &bull; صوت
        </p>

        <div style={{ display: "flex", gap: 28, justifyContent: "center", flexWrap: "wrap" }}>
          {[
            { icon: "🎬", label: "فيديوهات", href: "/video" },
            { icon: "🖼️", label: "صور",       href: "/gallery" },
            { icon: "🎙",  label: "صوت",       href: "/audio" },
          ].map(item => (
            <a key={item.label} href={item.href} style={{
              display: "flex", flexDirection: "column", alignItems: "center", gap: 6,
              textDecoration: "none",
            }}>
              <div style={{
                width: 52, height: 52, borderRadius: 14,
                background: "rgba(200,168,75,.15)",
                border: "1px solid rgba(200,168,75,.35)",
                display: "flex", alignItems: "center", justifyContent: "center",
                fontSize: 24,
              }}>{item.icon}</div>
              <span style={{ color: "rgba(255,255,255,.85)", fontSize: 12, fontWeight: 600 }}>{item.label}</span>
            </a>
          ))}
        </div>
      </div>
    </div>
  );
}
