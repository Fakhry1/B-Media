"use client";

import { useState, useEffect, useCallback, useRef } from "react";
import Header from "@/components/layout/Header";
import Footer from "@/components/layout/Footer";
import { fetchPublicContents, type PublicItem } from "@/lib/public";

/* ─── Static slides data ──────────────────────────────────── */
const SLIDES = [
  {
    bg: "linear-gradient(135deg,#0f2d1e 0%,#1a4332 40%,#0a1f12 100%)",
    accent: "#C8A84B",
    tag: "المشاهدة",
    icon: "🎬",
    title: "استمتع بأفضل مقاطع الفيديو",
    desc: "تصفّح مكتبة ضخمة من مقاطع الفيديو المنتقاة بعناية في مختلف الموضوعات والمجالات.",
    href: "/video",
    cta: "تصفح الفيديوهات",
    shape: (
      <svg viewBox="0 0 500 500" style={{ position: "absolute", left: "5%", top: "50%", transform: "translateY(-50%)", width: "min(420px,45vw)", opacity: .07 }}>
        <circle cx="250" cy="250" r="240" fill="none" stroke="#C8A84B" strokeWidth="2"/>
        <circle cx="250" cy="250" r="180" fill="none" stroke="#C8A84B" strokeWidth="1.5"/>
        <polygon points="190,140 380,250 190,360" fill="#C8A84B"/>
      </svg>
    ),
  },
  {
    bg: "linear-gradient(135deg,#1a1030 0%,#2d1a52 40%,#0f0a1f 100%)",
    accent: "#8B5CF6",
    tag: "السماع",
    icon: "🎧",
    title: "اسمع واستمتع بالمحتوى الصوتي",
    desc: "مجموعة متنوعة من المحتوى الصوتي الممتاز — محاضرات، دروس، وبرامج متخصصة.",
    href: "/audio",
    cta: "استمع الآن",
    shape: (
      <svg viewBox="0 0 500 500" style={{ position: "absolute", left: "5%", top: "50%", transform: "translateY(-50%)", width: "min(420px,45vw)", opacity: .07 }}>
        <circle cx="250" cy="250" r="240" fill="none" stroke="#8B5CF6" strokeWidth="2"/>
        <path d="M200 160 Q200 110 250 110 Q300 110 300 160 L300 250 Q300 300 250 300 Q200 300 200 250 Z" fill="#8B5CF6"/>
        <line x1="250" y1="300" x2="250" y2="350" stroke="#8B5CF6" strokeWidth="8"/>
        <line x1="200" y1="350" x2="300" y2="350" stroke="#8B5CF6" strokeWidth="8"/>
      </svg>
    ),
  },
  {
    bg: "linear-gradient(135deg,#1a2a10 0%,#2d4a1a 40%,#0f1f08 100%)",
    accent: "#10B981",
    tag: "الاطلاع",
    icon: "📖",
    title: "اقرأ واطّلع على أحدث المقالات",
    desc: "مقالات ومستندات متنوعة تغطي مختلف الموضوعات — أضف إلى معرفتك كل يوم.",
    href: "/articles",
    cta: "اقرأ الآن",
    shape: (
      <svg viewBox="0 0 500 500" style={{ position: "absolute", left: "5%", top: "50%", transform: "translateY(-50%)", width: "min(420px,45vw)", opacity: .07 }}>
        <circle cx="250" cy="250" r="240" fill="none" stroke="#10B981" strokeWidth="2"/>
        <rect x="130" y="120" width="240" height="300" rx="12" fill="#10B981"/>
        <line x1="170" y1="200" x2="330" y2="200" stroke="#0f1f08" strokeWidth="10"/>
        <line x1="170" y1="250" x2="330" y2="250" stroke="#0f1f08" strokeWidth="10"/>
        <line x1="170" y1="300" x2="260" y2="300" stroke="#0f1f08" strokeWidth="10"/>
      </svg>
    ),
  },
];

/* ─── Hero Slider (3 static slides) ─────────────────────────── */
function HeroSlider() {
  const [cur, setCur] = useState(0);
  const [paused, setPaused] = useState(false);
  const timerRef = useRef<ReturnType<typeof setInterval> | null>(null);

  const next = useCallback(() => setCur(c => (c + 1) % SLIDES.length), []);
  const prev = useCallback(() => setCur(c => (c - 1 + SLIDES.length) % SLIDES.length), []);

  useEffect(() => {
    if (paused) return;
    timerRef.current = setInterval(next, 5000);
    return () => { if (timerRef.current) clearInterval(timerRef.current); };
  }, [next, paused]);

  const slide = SLIDES[cur];

  return (
    <div
      style={{ position: "relative", height: 520, overflow: "hidden", userSelect: "none" }}
      onMouseEnter={() => setPaused(true)}
      onMouseLeave={() => setPaused(false)}
    >
      {/* Slides */}
      {SLIDES.map((s, i) => (
        <div key={i} style={{
          position: "absolute", inset: 0,
          background: s.bg,
          opacity: i === cur ? 1 : 0,
          transition: "opacity .9s ease",
          zIndex: i === cur ? 1 : 0,
        }}>
          {s.shape}
          <div style={{ position: "absolute", inset: 0, background: "linear-gradient(to left, transparent 30%, rgba(0,0,0,.6) 100%)" }} />
        </div>
      ))}

      {/* Content */}
      <div style={{ position: "absolute", inset: 0, zIndex: 2, display: "flex", flexDirection: "column", justifyContent: "center" }}>
        <div style={{ maxWidth: 1280, margin: "0 auto", width: "100%", padding: "0 32px" }}>
          <div style={{ maxWidth: 580 }}>
            <div style={{ display: "inline-flex", alignItems: "center", gap: 8, background: `${slide.accent}22`, border: `1px solid ${slide.accent}55`, color: slide.accent, fontSize: 12, fontWeight: 700, padding: "5px 14px", borderRadius: 20, marginBottom: 18 }}>
              <span>{slide.icon}</span> {slide.tag}
            </div>
            <h1 style={{
              color: "#fff", fontSize: "clamp(24px,4vw,42px)", fontWeight: 800, lineHeight: 1.3,
              margin: "0 0 16px", fontFamily: "'Noto Kufi Arabic',sans-serif",
              textShadow: "0 2px 16px rgba(0,0,0,.5)",
            }}>
              {slide.title}
            </h1>
            <p style={{ color: "rgba(255,255,255,.72)", fontSize: 16, lineHeight: 1.7, marginBottom: 28, maxWidth: 480 }}>
              {slide.desc}
            </p>
            <SlideBtn href={slide.href} label={slide.cta} color={slide.accent} />
          </div>
        </div>
      </div>

      {/* Progress bar */}
      {!paused && (
        <div style={{ position: "absolute", bottom: 0, left: 0, right: 0, height: 3, zIndex: 3, background: "rgba(255,255,255,.12)" }}>
          <div key={cur} style={{ height: "100%", background: slide.accent, animation: "progress 5s linear forwards" }} />
        </div>
      )}

      {/* Counter */}
      <div style={{ position: "absolute", top: 20, left: 20, zIndex: 3, background: "rgba(0,0,0,.45)", backdropFilter: "blur(10px)", color: "#fff", fontSize: 12, fontWeight: 600, padding: "5px 13px", borderRadius: 20, border: "1px solid rgba(255,255,255,.15)" }}>
        {cur + 1} / {SLIDES.length}
      </div>

      <ArrowBtn side="right" onClick={prev} />
      <ArrowBtn side="left"  onClick={next} />

      {/* Dots */}
      <div style={{ position: "absolute", bottom: 18, left: "50%", transform: "translateX(-50%)", zIndex: 3, display: "flex", gap: 7, alignItems: "center" }}>
        {SLIDES.map((s, i) => (
          <button key={i} onClick={() => setCur(i)} style={{
            width: i === cur ? 28 : 8, height: 8, borderRadius: 4, border: "none",
            background: i === cur ? s.accent : "rgba(255,255,255,.35)",
            cursor: "pointer", transition: "all .35s", padding: 0,
          }} />
        ))}
      </div>

      <style>{`
        @keyframes progress { from { width: 0% } to { width: 100% } }
      `}</style>
    </div>
  );
}

function SlideBtn({ href, label, color }: { href: string; label: string; color: string }) {
  const [hover, setHover] = useState(false);
  return (
    <a href={href}
      onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{
        display: "inline-flex", alignItems: "center", gap: 8,
        padding: "12px 28px", borderRadius: 14,
        background: hover ? color : `${color}dd`,
        color: "#fff", fontSize: 14, fontWeight: 700, textDecoration: "none",
        transform: hover ? "translateY(-2px)" : "none", transition: "all .2s",
        boxShadow: hover ? `0 8px 24px ${color}55` : "none",
        border: `1px solid ${color}`,
      }}>
      {label}
      <svg width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
        <path d="M5 12h14M12 5l7 7-7 7" />
      </svg>
    </a>
  );
}

function ArrowBtn({ side, onClick }: { side: "left" | "right"; onClick: () => void }) {
  const [hover, setHover] = useState(false);
  return (
    <button onClick={onClick}
      onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{
        position: "absolute", top: "50%", [side]: 20, transform: "translateY(-50%)", zIndex: 3,
        width: 46, height: 46, borderRadius: "50%", border: "1px solid rgba(255,255,255,.25)",
        background: hover ? "rgba(0,0,0,.65)" : "rgba(0,0,0,.35)",
        color: "#fff", cursor: "pointer", backdropFilter: "blur(10px)",
        display: "flex", alignItems: "center", justifyContent: "center",
        transition: "background .2s", fontSize: 22, fontWeight: 300,
      }}>
      {side === "right" ? "›" : "‹"}
    </button>
  );
}

/* ─── Quick Links ─────────────────────────────────────────── */
const QUICK_LINKS = [
  { href: "/articles", icon: "📖", label: "الاطلاع",  sub: "مقالات ومستندات", color: "#3B82F6" },
  { href: "/audio",    icon: "🎧", label: "السماع",   sub: "محتوى صوتي",       color: "#8B5CF6" },
  { href: "/video",    icon: "🎬", label: "المشاهدة", sub: "مقاطع فيديو",      color: "#10B981" },
  { href: "/gallery",  icon: "🖼️", label: "صور",      sub: "معرض الصور",       color: "#F59E0B" },
];

function QuickLinkCard({ href, icon, label, sub, color }: typeof QUICK_LINKS[0]) {
  const [hover, setHover] = useState(false);
  return (
    <a href={href}
      onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{
        display: "flex", alignItems: "center", gap: 14, padding: "18px 20px",
        borderRadius: 18, border: `1px solid ${hover ? color + "44" : "var(--line)"}`,
        background: hover ? `${color}0c` : "var(--surface)", textDecoration: "none",
        transition: "all .2s", boxShadow: hover ? "var(--shadow-md)" : "var(--shadow-sm)",
        transform: hover ? "translateY(-2px)" : "none",
      }}>
      <div style={{ width: 48, height: 48, borderRadius: 14, background: `${color}18`, display: "flex", alignItems: "center", justifyContent: "center", fontSize: 24, flexShrink: 0 }}>
        {icon}
      </div>
      <div>
        <p style={{ color: "var(--ink)", fontWeight: 700, fontSize: 15, margin: 0 }}>{label}</p>
        <p style={{ color: "var(--muted)", fontSize: 12, margin: "3px 0 0" }}>{sub}</p>
      </div>
      <svg style={{ marginRight: "auto", color: hover ? color : "var(--muted-2)", transition: "color .2s, transform .2s", transform: hover ? "translateX(-3px)" : "none" }}
        width="18" height="18" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2">
        <path d="M5 12h14M12 5l7 7-7 7" />
      </svg>
    </a>
  );
}

function QuickLinks() {
  return (
    <section style={{ maxWidth: 1280, margin: "0 auto", padding: "36px 24px 0" }}>
      <div className="ql-grid">
        {QUICK_LINKS.map(l => <QuickLinkCard key={l.href} {...l} />)}
      </div>
    </section>
  );
}

/* ─── Mini Card ───────────────────────────────────────────── */
const ACCENT = ["#3B82F6","#8B5CF6","#10B981","#F59E0B","#EC4899","#06B6D4","#EF4444"];
const accentFor = (t: string) => ACCENT[t.charCodeAt(0) % ACCENT.length];

const TYPE_ICONS: Record<number, JSX.Element> = {
  1: <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"><polygon points="5,3 19,12 5,21" /></svg>,
  2: <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"><rect x="3" y="3" width="18" height="18" rx="2"/><circle cx="8.5" cy="8.5" r="1.5"/><path d="M21 15l-5-5L5 21"/></svg>,
  3: <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M9 18V5l12-2v13"/><circle cx="6" cy="18" r="3"/><circle cx="18" cy="16" r="3"/></svg>,
  5: <svg width="26" height="26" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="1.5"><path d="M14 2H6a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h12a2 2 0 0 0 2-2V8z"/><polyline points="14 2 14 8 20 8"/></svg>,
};

function MiniCard({ item, mediaType, href }: { item: PublicItem; mediaType: number; href: string }) {
  const [hover, setHover] = useState(false);
  const c = accentFor(item.title);
  return (
    <a href={href}
      onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{
        flexShrink: 0, width: 210, borderRadius: 16, overflow: "hidden",
        background: "var(--surface)", border: `1px solid ${hover ? c + "44" : "var(--line)"}`,
        textDecoration: "none", display: "block",
        boxShadow: hover ? "var(--shadow-md)" : "var(--shadow-sm)",
        transform: hover ? "translateY(-4px)" : "none", transition: "all .2s",
      }}>
      <div style={{ position: "relative", height: 126, overflow: "hidden", background: `linear-gradient(135deg,${c}18,${c}30)` }}>
        {item.thumbnailUrl
          ? <img src={item.thumbnailUrl} alt={item.title} loading="lazy"
              style={{ width: "100%", height: "100%", objectFit: "cover", transform: hover ? "scale(1.07)" : "scale(1)", transition: "transform .35s" }} />
          : <div style={{ height: "100%", display: "flex", alignItems: "center", justifyContent: "center", color: c }}>
              {TYPE_ICONS[mediaType]}
            </div>
        }
        {mediaType === 1 && (
          <div style={{ position: "absolute", inset: 0, display: "flex", alignItems: "center", justifyContent: "center", background: "rgba(0,0,0,.18)" }}>
            <div style={{ width: 34, height: 34, borderRadius: "50%", background: "rgba(255,255,255,.88)", display: "flex", alignItems: "center", justifyContent: "center" }}>
              <svg width="12" height="12" viewBox="0 0 24 24" fill="#1a1a1a" style={{ marginRight: -2 }}><polygon points="5,3 19,12 5,21" /></svg>
            </div>
          </div>
        )}
        {item.isFeatured && (
          <div style={{ position: "absolute", top: 6, right: 6, background: "var(--gold)", color: "var(--forest)", fontSize: 10, fontWeight: 700, padding: "2px 8px", borderRadius: 20 }}>مميز</div>
        )}
      </div>
      <div style={{ padding: "10px 12px" }}>
        <p style={{ color: "var(--ink)", fontSize: 13, fontWeight: 700, lineHeight: 1.4, margin: "0 0 4px", overflow: "hidden", textOverflow: "ellipsis", whiteSpace: "nowrap" }}>
          {item.title}
        </p>
        {item.categoryName && <p style={{ color: c, fontSize: 11, fontWeight: 600, margin: 0 }}>{item.categoryName}</p>}
      </div>
    </a>
  );
}

/* ─── Section Strip ───────────────────────────────────────── */
function SectionStrip({ title, icon, href, mediaType }: { title: string; icon: string; href: string; mediaType: number }) {
  const [items, setItems] = useState<PublicItem[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const ctrl = new AbortController();
    fetchPublicContents({ pageSize: 6, mediaType }, ctrl.signal)
      .then(d => { setItems(d.items); setLoading(false); })
      .catch(() => setLoading(false));
    return () => ctrl.abort();
  }, [mediaType]);

  return (
    <section style={{ padding: "40px 0 0" }}>
      <div style={{ maxWidth: 1280, margin: "0 auto", padding: "0 24px" }}>
        {/* Header */}
        <div style={{ display: "flex", alignItems: "center", justifyContent: "space-between", marginBottom: 20 }}>
          <h2 style={{ color: "var(--ink)", fontWeight: 800, fontSize: 20, fontFamily: "'Noto Kufi Arabic',sans-serif", display: "flex", alignItems: "center", gap: 8, margin: 0 }}>
            <span>{icon}</span> {title}
          </h2>
          <SeeAllLink href={href} />
        </div>

        {/* Horizontal scroll row */}
        <div style={{ display: "flex", gap: 16, overflowX: "auto", paddingBottom: 12, scrollbarWidth: "none" }}>
          {loading
            ? Array.from({ length: 5 }).map((_, i) => <SkeletonCard key={i} />)
            : items.length === 0
              ? <p style={{ color: "var(--muted)", fontSize: 14, padding: "20px 0" }}>لا يوجد محتوى حالياً</p>
              : items.map(item => <MiniCard key={item.id} item={item} mediaType={mediaType} href={href} />)
          }
        </div>

        {/* Divider */}
        <div style={{ height: 1, background: "var(--line)", marginTop: 20 }} />
      </div>
    </section>
  );
}

function SeeAllLink({ href }: { href: string }) {
  const [hover, setHover] = useState(false);
  return (
    <a href={href}
      onMouseEnter={() => setHover(true)} onMouseLeave={() => setHover(false)}
      style={{ color: hover ? "var(--forest)" : "var(--muted)", fontSize: 13, fontWeight: 600, textDecoration: "none", display: "flex", alignItems: "center", gap: 5, transition: "color .15s" }}>
      رؤية الكل
      <svg width="14" height="14" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2.5">
        <path d="M5 12h14M12 5l7 7-7 7" />
      </svg>
    </a>
  );
}

function SkeletonCard() {
  return (
    <div className="animate-pulse" style={{ flexShrink: 0, width: 210, borderRadius: 16, overflow: "hidden", background: "var(--surface)", border: "1px solid var(--line)" }}>
      <div style={{ height: 126, background: "var(--surface-2)" }} />
      <div style={{ padding: "10px 12px" }}>
        <div style={{ height: 13, borderRadius: 4, background: "var(--surface-2)", width: "80%", marginBottom: 6 }} />
        <div style={{ height: 11, borderRadius: 4, background: "var(--surface-2)", width: "50%" }} />
      </div>
    </div>
  );
}

/* ─── Page ────────────────────────────────────────────────── */
export default function HomePage() {
  return (
    <div style={{ minHeight: "100vh", display: "flex", flexDirection: "column", background: "var(--bg)" }}>
      <Header />
      <main style={{ flex: 1 }}>
        <HeroSlider />
        <QuickLinks />
        <SectionStrip title="المشاهدة" icon="🎬" href="/video"    mediaType={1} />
        <SectionStrip title="السماع"   icon="🎧" href="/audio"    mediaType={3} />
        <SectionStrip title="الاطلاع"  icon="📖" href="/articles" mediaType={5} />
        <SectionStrip title="صور"      icon="🖼️" href="/gallery"  mediaType={2} />
        <div style={{ height: 64 }} />
      </main>
      <Footer />

      <style>{`
        .ql-grid { display: grid; gap: 16px; grid-template-columns: repeat(4,1fr); }
        @media (max-width: 900px) { .ql-grid { grid-template-columns: repeat(2,1fr); } }
        @media (max-width: 480px) { .ql-grid { grid-template-columns: 1fr 1fr; } }
      `}</style>
    </div>
  );
}
