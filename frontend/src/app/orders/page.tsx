"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function OrdersPage() {
  const [orders, setOrders] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [orderNumber, setOrderNumber] = useState("");
  const [customerName, setCustomerName] = useState("");
  const [destination, setDestination] = useState("");
  const [weightKg, setWeightKg] = useState(100);
  const [isLoading, setIsLoading] = useState(false);

  const fetchOrders = async () => {
    try {
      const res = await axios.get("http://localhost:5243/api/Order");
      setOrders(res.data);
    } catch (err) {
      console.warn("Siparişler çekilemedi:", err);
    }
  };

  useEffect(() => { fetchOrders(); }, []);

  const handleAddOrder = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      await axios.post("http://localhost:5243/api/Order", {
        orderNumber,
        customerName,
        destination,
        weightKg: Number(weightKg)
      });
      
      await fetchOrders();
      setIsOpen(false);
      setOrderNumber(""); setCustomerName(""); setDestination(""); setWeightKg(100);
    } catch (error) {
      alert("Sipariş oluşturulurken bir hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">📦 Sipariş Yönetimi</h1>
          <p className="text-slate-600 text-sm">Müşterilerden gelen lojistik siparişlerin takibi ve sevk hazırlığı.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>+</span> Yeni Sipariş Ekle
        </button>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 bg-slate-50">
          <span className="text-sm text-slate-500 font-medium">Toplam: {orders.length} Kayıtlı Sipariş (SQL Database)</span>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Sipariş No</th>
              <th className="p-4 font-semibold">Müşteri</th>
              <th className="p-4 font-semibold">Varış Noktası</th>
              <th className="p-4 font-semibold">Ağırlık (KG)</th>
              <th className="p-4 font-semibold">Durum</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {orders.length === 0 ? (
              <tr><td colSpan={5} className="p-8 text-center text-slate-400">Veritabanında kayıtlı sipariş bulunamadı.</td></tr>
            ) : (
              orders.map((o) => (
                <tr key={o.id} className="hover:bg-slate-50/50 transition-colors">
                  <td className="p-4 font-bold text-slate-900">{o.orderNumber}</td>
                  <td className="p-4">{o.customerName}</td>
                  <td className="p-4">{o.destination}</td>
                  <td className="p-4">{o.weightKg} kg</td>
                  <td className="p-4">
                    <span className="px-3 py-1 bg-amber-50 text-amber-600 rounded-full text-xs font-bold">{o.status}</span>
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
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Sipariş Girişi</h2>
            <form onSubmit={handleAddOrder} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Sipariş No</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={orderNumber} onChange={(e) => setOrderNumber(e.target.value)} required placeholder="ORD-2026-001" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Müşteri Adı</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={customerName} onChange={(e) => setCustomerName(e.target.value)} required placeholder="ABC Tekstil A.Ş." />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Varış Noktası (Adres)</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={destination} onChange={(e) => setDestination(e.target.value)} required placeholder="Ankara Merkez Depo" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Ağırlık (KG)</label>
                <input type="number" className="w-full px-3 py-2 border rounded-xl text-sm" value={weightKg} onChange={(e) => setWeightKg(Number(e.target.value))} required />
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button type="button" onClick={() => setIsOpen(false)} className="px-4 py-2 border rounded-xl text-sm text-slate-600 hover:bg-slate-50 font-medium">İptal</button>
                <button type="submit" disabled={isLoading} className="px-5 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-xl text-sm font-bold disabled:opacity-50">
                  {isLoading ? "Kaydediliyor..." : "Sipariş Oluştur"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
