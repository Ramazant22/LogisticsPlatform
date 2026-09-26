"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function ShipmentsPage() {
  const [shipments, setShipments] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const fetchShipments = async () => {
      try {
        const res = await axios.get("http://localhost:5243/api/Shipment");
        setShipments(res.data);
      } catch (error) {
        console.warn("Shipment API hatası");
      }
    };
    fetchShipments();
  }, []);

  const handleStatusChange = async (trackingNumber: string, newStatus: string) => {
    setLoading(true);
    
    // Arayüzü anında güncelle (Kullanıcıyı bekletmemek için Optimistic Update)
    setShipments(prev => prev.map(s => s.trackingNumber === trackingNumber ? { ...s, status: newStatus } : s));

    try {
      // Backend'e kesin, kontrollü durum güncellemesi gönder
      await axios.post("http://localhost:5243/api/Shipment/updateStatus", {
        trackingNumber,
        newStatus
      });
    } catch (err) {
      console.warn("Durum güncellenirken hata oluştu.");
    } finally {
      setLoading(false);
    }
  };

  const getStatusColor = (status: string) => {
    if (status === "Hazırlanıyor") return "bg-orange-100 text-orange-700";
    if (status === "Yolda") return "bg-blue-100 text-blue-700";
    if (status === "Teslim Edildi") return "bg-green-100 text-green-700";
    return "bg-slate-100 text-slate-700";
  };

  return (
    <div className="p-8">
      <div className="mb-8">
        <h1 className="text-2xl font-bold text-slate-800 flex items-center gap-2">📦 Teslimat ve Kargo Operasyonları</h1>
        <p className="text-slate-600 text-sm mt-1">Sistemdeki kargoların anlık durumlarını güvenli ve detaylı liste görünümü ile yönetin.</p>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <input type="text" placeholder="Takip No veya Firma ara..." className="px-4 py-2 border rounded-xl w-72 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 bg-white" />
          <span className="text-sm text-slate-500 font-medium">Toplam: {shipments.length} Kayıt</span>
        </div>
        
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-500 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Takip No</th>
              <th className="p-4 font-semibold">Firma</th>
              <th className="p-4 font-semibold">Rota</th>
              <th className="p-4 font-semibold">Ağırlık</th>
              <th className="p-4 font-semibold">Mevcut Durum</th>
              <th className="p-4 font-semibold">İşlem (Durum Güncelle)</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {shipments.map((s) => (
              <tr key={s.trackingNumber} className="hover:bg-slate-50/50 transition-colors">
                <td className="p-4 font-bold text-slate-900">{s.trackingNumber}</td>
                <td className="p-4">{s.company}</td>
                <td className="p-4 flex items-center gap-2">
                  <span className="text-slate-400 font-medium">{s.from}</span>
                  <span className="text-slate-300">→</span>
                  <span className="text-slate-400 font-medium">{s.to}</span>
                </td>
                <td className="p-4">{s.weight}</td>
                <td className="p-4">
                  <span className={`px-3 py-1 rounded-full text-xs font-bold ${getStatusColor(s.status)}`}>
                    {s.status}
                  </span>
                </td>
                <td className="p-4">
                  <select 
                    value={s.status}
                    onChange={(e) => handleStatusChange(s.trackingNumber, e.target.value)}
                    disabled={loading}
                    className="px-3 py-2 border rounded-lg text-sm bg-white font-medium text-slate-700 focus:outline-none focus:ring-2 focus:ring-blue-500 disabled:opacity-50 cursor-pointer w-36"
                  >
                    <option value="Hazırlanıyor">Hazırlanıyor</option>
                    <option value="Yolda">Yolda</option>
                    <option value="Teslim Edildi">Teslim Edildi</option>
                  </select>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}
