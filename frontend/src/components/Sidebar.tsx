"use client";
import Link from "next/link";
import { usePathname, useRouter } from "next/navigation";

export default function Sidebar() {
  const pathname = usePathname();
  const router = useRouter();

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("user");
    localStorage.removeItem("hasSeenOnboarding");
    router.replace("/login");
  };

  const menuSections = [
    {
      title: "GÖSTERGE PANELİ",
      items: [
        { name: "Dashboard", path: "/dashboard", icon: "📊" },
        { name: "Bildirimler", path: "/notifications", icon: "🔔" },
      ],
    },
    {
      title: "OPERASYON",
      items: [
        { name: "Siparişler (Orders)", path: "/orders", icon: "📦" },
        { name: "Teslimatlar (Shipment)", path: "/shipments", icon: "🚚" },
        { name: "Rotalar (Routes)", path: "/routes", icon: "🗺️" },
        { name: "Canlı Takip (Tracking)", path: "/tracking", icon: "📍" },
      ],
    },
    {
      title: "KAYNAK YÖNETİMİ",
      items: [
        { name: "Filo Yönetimi (Fleet)", path: "/fleet", icon: "🚛" },
        { name: "Sürücüler (Drivers)", path: "/drivers", icon: "👨‍✈️" },
        { name: "Bakım Yönetimi", path: "/maintenance", icon: "🔧" },
      ],
    },
    {
      title: "FİNANS VE CRM",
      items: [
        { name: "Müşteriler (Customers)", path: "/customers", icon: "🏢" },
        { name: "Finans (Billing)", path: "/billing", icon: "💲" },
        { name: "Raporlar", path: "/reporting", icon: "📈" },
      ],
    },
    {
      title: "SİSTEM VE ERİŞİM",
      items: [
        { name: "AI Operasyon Asistanı", path: "/ai-assistant", icon: "✨" },
        { name: "Kimlik Yönetimi (Identity)", path: "/identity", icon: "🛡️" },
        { name: "Firmalar (Tenancy)", path: "/tenancy", icon: "🏢" },
      ],
    },
  ];

  return (
    <div className="w-64 bg-slate-900 text-slate-300 min-h-screen flex flex-col justify-between">
      <div className="overflow-y-auto">
        <h1 className="text-2xl font-bold text-white p-6 tracking-wider">LOGISTICS<span className="text-blue-500">.AI</span></h1>
        <nav className="space-y-6 px-4 pb-6">
          {menuSections.map((section, idx) => (
            <div key={idx}>
              <h3 className="text-xs font-semibold text-slate-500 uppercase tracking-wider mb-3 px-3">
                {section.title}
              </h3>
              <ul className="space-y-1">
                {section.items.map((item) => (
                  <li key={item.path}>
                    <Link
                      href={item.path}
                      className={`flex items-center gap-3 px-3 py-2 rounded-lg transition-colors text-sm font-medium ${
                        pathname === item.path
                          ? "bg-blue-600 text-white"
                          : "hover:bg-slate-800 hover:text-white text-slate-400"
                      }`}
                    >
                      <span>{item.icon}</span>
                      {item.name}
                    </Link>
                  </li>
                ))}
              </ul>
            </div>
          ))}
        </nav>
      </div>
      
      {/* Çıkış Yap Butonu Alt Kısma Eklendi */}
      <div className="p-4 border-t border-slate-800 bg-slate-900 sticky bottom-0">
        <button 
          onClick={handleLogout}
          className="flex w-full items-center justify-center gap-2 px-4 py-2 bg-red-500/10 text-red-400 hover:bg-red-500 hover:text-white rounded-lg transition-all text-sm font-bold"
        >
          <span>🚪</span> Çıkış Yap
        </button>
      </div>
    </div>
  );
}
