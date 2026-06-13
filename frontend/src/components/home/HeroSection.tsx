"use client";

import HeroBannerSlider from "./HeroBannerSlider";

export default function HeroSection() {
  return (
    <section className="pt-8 pb-4">
      <div className="container-main">
        {/* Banner slider */}
        <HeroBannerSlider />

        {/* Categories */}
        <div className="mt-4 pb-2">
          <div className="flex gap-3 overflow-x-auto pb-1" style={{ scrollSnapType: "x mandatory" }}>
            {[
              { icon: "🎬", label: "فيديو", sub: "محاضرات وبرامج", href: "/video" },
              { icon: "🖼", label: "صور", sub: "معرض بصري", href: "/gallery" },
              { icon: "🎙", label: "صوت", sub: "تسجيلات وأناشيد", href: "/audio" },
              { icon: "📖", label: "قراءة", sub: "مقالات وكتب", href: "/articles" },
              { icon: "📂", label: "التصنيفات", sub: "تصفح حسب القسم", href: "/categories" },
            ].map((c, i) => (
              <a key={c.label} href={c.href}
                className="flex-shrink-0 flex items-center gap-3 px-4 py-3 rounded-2xl border transition-all cursor-pointer"
                style={{ background: "var(--surface)", borderColor: "var(--line)", boxShadow: "var(--shadow-sm)", minWidth: "160px", scrollSnapAlign: "start" }}
                onMouseEnter={e => { (e.currentTarget as HTMLElement).style.borderColor = "var(--gold)"; (e.currentTarget as HTMLElement).style.transform = "translateY(-3px)"; }}
                onMouseLeave={e => { (e.currentTarget as HTMLElement).style.borderColor = "var(--line)"; (e.currentTarget as HTMLElement).style.transform = ""; }}>
                <div className="w-12 h-12 rounded-xl flex items-center justify-center text-xl flex-shrink-0"
                  style={{ background: i % 2 === 0 ? "rgba(26,67,50,.10)" : "rgba(200,168,75,.12)" }}>
                  {c.icon}
                </div>
                <div>
                  <b className="block text-sm font-bold" style={{ color: "var(--ink)", fontFamily: "'Noto Kufi Arabic',sans-serif" }}>{c.label}</b>
                  <span className="block text-xs mt-0.5" style={{ color: "var(--muted)" }}>{c.sub}</span>
                </div>
              </a>
            ))}
          </div>
        </div>
      </div>
    </section>
  );
}

