"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function ReportingPage() {
  const [reports, setReports] = useState<any[]>([]);
  const [isLoading, setIsLoading] = useState(false);

  const fetchReports = async () => {
    try {
      const res = await axios.get("http://localhost:5243/api/Reporting");
      setReports(res.data);
    } catch (err) {
      console.warn("Raporlar çekilemedi:", err);
    }
  };

  useEffect(() => { fetchReports(); }, []);

  const handleGenerateReport = async () => {
    setIsLoading(true);
    try {
      // Demo amaçlı rastgele verilerle rapor oluşturuyoruz
      await axios.post("http://localhost:5243/api/Reporting", {
        totalShipments: Math.floor(Math.random() * 500) + 100,
        deliveredShipments: Math.floor(Math.random() * 400) + 50,
        activeVehicles: Math.floor(Math.random() * 50) + 10,
        totalRevenue: Math.floor(Math.random() * 50000) + 10000
      });
      await fetchReports();
    } catch (error) {
      alert("Rapor oluşturulurken hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-extrabold text-slate-800">📊 Lojistik Analiz ve Raporlama</h1>
          <p className="text-slate-500 mt-1">Platformun günlük operasyonel ve finansal özetleri.</p>
        </div>
        <button 
          onClick={handleGenerateReport}
          disabled={isLoading}
          className="bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 px-6 rounded-xl shadow-lg transition-all flex items-center gap-2 disabled:opacity-50"
        >
          {isLoading ? "Hesaplanıyor..." : "📈 Yeni Günlük Rapor Oluştur"}
        </button>
      </div>

      <div className="grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-4 gap-6 mb-8">
        {reports.length > 0 && (
          <>
            <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex flex-col gap-2">
              <span className="text-sm font-bold text-slate-400 uppercase">Toplam Gönderi</span>
              <span className="text-4xl font-black text-slate-800">{reports[0].totalShipments}</span>
              <span className="text-xs font-semibold text-emerald-500 bg-emerald-50 px-2 py-1 rounded w-fit">+ %12 Artış</span>
            </div>
            <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex flex-col gap-2">
              <span className="text-sm font-bold text-slate-400 uppercase">Teslim Edilen</span>
              <span className="text-4xl font-black text-blue-600">{reports[0].deliveredShipments}</span>
              <span className="text-xs font-semibold text-slate-500 bg-slate-100 px-2 py-1 rounded w-fit">Teslimat Oranı: %{((reports[0].deliveredShipments / reports[0].totalShipments) * 100).toFixed(0)}</span>
            </div>
            <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex flex-col gap-2">
              <span className="text-sm font-bold text-slate-400 uppercase">Aktif Araçlar</span>
              <span className="text-4xl font-black text-amber-500">{reports[0].activeVehicles}</span>
              <span className="text-xs font-semibold text-amber-700 bg-amber-50 px-2 py-1 rounded w-fit">Sahada</span>
            </div>
            <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-100 flex flex-col gap-2">
              <span className="text-sm font-bold text-slate-400 uppercase">Günlük Gelir</span>
              <span className="text-4xl font-black text-emerald-600">₺{reports[0].totalRevenue.toLocaleString()}</span>
              <span className="text-xs font-semibold text-emerald-700 bg-emerald-50 px-2 py-1 rounded w-fit">Güncellendi</span>
            </div>
          </>
        )}
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-5 border-b border-slate-100 bg-slate-50 flex justify-between items-center">
          <h3 className="font-bold text-slate-700">Geçmiş Raporlar (Son 10 Gün)</h3>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-white text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Tarih</th>
              <th className="p-4 font-semibold">Gönderi / Teslimat</th>
              <th className="p-4 font-semibold">Aktif Araç</th>
              <th className="p-4 font-semibold text-right">Gelir</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {reports.length === 0 ? (
              <tr><td colSpan={4} className="p-8 text-center text-slate-400">Henüz rapor oluşturulmadı.</td></tr>
            ) : (
              reports.map((r) => (
                <tr key={r.id} className="hover:bg-slate-50 transition-colors">
                  <td className="p-4 font-bold text-slate-800">{new Date(r.reportDate).toLocaleDateString()}</td>
                  <td className="p-4 font-medium">{r.totalShipments} / <span className="text-blue-600">{r.deliveredShipments}</span></td>
                  <td className="p-4"><span className="px-3 py-1 bg-slate-100 rounded-full font-bold">{r.activeVehicles}</span></td>
                  <td className="p-4 font-bold text-emerald-600 text-right">₺{r.totalRevenue.toLocaleString()}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
}
