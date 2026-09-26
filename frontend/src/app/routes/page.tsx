"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function RoutesPage() {
  const [routes, setRoutes] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [routeName, setRouteName] = useState("");
  const [startLocation, setStartLocation] = useState("");
  const [endLocation, setEndLocation] = useState("");
  const [distanceKm, setDistanceKm] = useState(450);
  const [isLoading, setIsLoading] = useState(false);

  const fetchRoutes = async () => {
    try {
      const res = await axios.get("http://localhost:5243/api/Route");
      setRoutes(res.data);
    } catch (err) {
      console.warn("Rotalar çekilemedi:", err);
    }
  };

  useEffect(() => { fetchRoutes(); }, []);

  const handleAddRoute = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      await axios.post("http://localhost:5243/api/Route", {
        routeName,
        startLocation,
        endLocation,
        distanceKm: Number(distanceKm)
      });
      
      await fetchRoutes();
      setIsOpen(false);
      setRouteName(""); setStartLocation(""); setEndLocation(""); setDistanceKm(450);
    } catch (error) {
      alert("Rota kaydedilirken bir hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">🗺️ Rota ve Güzergah Yönetimi</h1>
          <p className="text-slate-600 text-sm">Lojistik taşımalarda kullanılan ana güzergahların ve mesafelerin takibi.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>+</span> Yeni Rota Ekle
        </button>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 bg-slate-50">
          <span className="text-sm text-slate-500 font-medium">Toplam: {routes.length} Kayıtlı Rota (SQL Database)</span>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Rota Adı</th>
              <th className="p-4 font-semibold">Başlangıç</th>
              <th className="p-4 font-semibold">Bitiş</th>
              <th className="p-4 font-semibold">Mesafe</th>
              <th className="p-4 font-semibold">Durum</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {routes.length === 0 ? (
              <tr><td colSpan={5} className="p-8 text-center text-slate-400">Veritabanında kayıtlı rota bulunamadı.</td></tr>
            ) : (
              routes.map((r) => (
                <tr key={r.id} className="hover:bg-slate-50/50 transition-colors">
                  <td className="p-4 font-bold text-slate-900">{r.routeName}</td>
                  <td className="p-4">{r.startLocation}</td>
                  <td className="p-4">{r.endLocation}</td>
                  <td className="p-4 font-semibold text-slate-600">{r.distanceKm} km</td>
                  <td className="p-4">
                    <span className="px-3 py-1 bg-emerald-50 text-emerald-600 rounded-full text-xs font-bold">{r.status}</span>
                  </td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {isOpen && (
        <div className="fixed inset-0 bg-black/50 z-50 flex items-center justify-center p-4">
          <div className="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6">
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Rota Tanımla</h2>
            <form onSubmit={handleAddRoute} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Rota Adı</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={routeName} onChange={(e) => setRouteName(e.target.value)} required placeholder="İstanbul - Ankara Ekspres" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Başlangıç Noktası</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={startLocation} onChange={(e) => setStartLocation(e.target.value)} required placeholder="İstanbul" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Bitiş Noktası</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={endLocation} onChange={(e) => setEndLocation(e.target.value)} required placeholder="Ankara" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Mesafe (KM)</label>
                <input type="number" className="w-full px-3 py-2 border rounded-xl text-sm" value={distanceKm} onChange={(e) => setDistanceKm(Number(e.target.value))} required />
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button type="button" onClick={() => setIsOpen(false)} className="px-4 py-2 border rounded-xl text-sm text-slate-600 hover:bg-slate-50 font-medium">İptal</button>
                <button type="submit" disabled={isLoading} className="px-5 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-xl text-sm font-bold disabled:opacity-50">
                  {isLoading ? "Kaydediliyor..." : "Rotayı Kaydet"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
