"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function ShipmentsPage() {
  const [shipments, setShipments] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [trackingNumber, setTrackingNumber] = useState("");
  const [origin, setOrigin] = useState("");
  const [destination, setDestination] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const fetchShipments = async () => {
    try {
      const res = await axios.get("http://localhost:5243/api/Shipment");
      setShipments(res.data);
    } catch (err) {
      console.warn("Gönderiler çekilemedi:", err);
    }
  };

  useEffect(() => { fetchShipments(); }, []);

  const handleAddShipment = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      await axios.post("http://localhost:5243/api/Shipment", {
        trackingNumber,
        origin,
        destination
      });
      
      await fetchShipments();
      setIsOpen(false);
      setTrackingNumber(""); setOrigin(""); setDestination("");
    } catch (error) {
      alert("Gönderi kaydedilirken bir hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">📦 Gönderiler ve Sevkıyat</h1>
          <p className="text-slate-600 text-sm">Saha operasyonundaki taşımaların ve kargo sevklerinin takibi.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>+</span> Yeni Gönderi Ekle
        </button>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 bg-slate-50">
          <span className="text-sm text-slate-500 font-medium">Toplam: {shipments.length} Kayıtlı Gönderi (SQL Database)</span>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Takip Numarası</th>
              <th className="p-4 font-semibold">Çıkış Noktası</th>
              <th className="p-4 font-semibold">Varış Noktası</th>
              <th className="p-4 font-semibold">Durum</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {shipments.length === 0 ? (
              <tr><td colSpan={4} className="p-8 text-center text-slate-400">Veritabanında kayıtlı gönderi bulunamadı.</td></tr>
            ) : (
              shipments.map((s) => (
                <tr key={s.id} className="hover:bg-slate-50/50 transition-colors">
                  <td className="p-4 font-bold text-slate-900">{s.trackingNumber}</td>
                  <td className="p-4">{s.origin}</td>
                  <td className="p-4">{s.destination}</td>
                  <td className="p-4">
                    <span className="px-3 py-1 bg-blue-50 text-blue-600 rounded-full text-xs font-bold">{s.status}</span>
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
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Gönderi Kaydı</h2>
            <form onSubmit={handleAddShipment} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Takip Numarası</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={trackingNumber} onChange={(e) => setTrackingNumber(e.target.value)} required placeholder="TRK-987654" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Çıkış Noktası</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={origin} onChange={(e) => setOrigin(e.target.value)} required placeholder="İstanbul Ana Depo" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Varış Noktası</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={destination} onChange={(e) => setDestination(e.target.value)} required placeholder="İzmir Şube" />
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button type="button" onClick={() => setIsOpen(false)} className="px-4 py-2 border rounded-xl text-sm text-slate-600 hover:bg-slate-50 font-medium">İptal</button>
                <button type="submit" disabled={isLoading} className="px-5 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-xl text-sm font-bold disabled:opacity-50">
                  {isLoading ? "Kaydediliyor..." : "Kaydet"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
