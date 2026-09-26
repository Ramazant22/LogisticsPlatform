"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function FleetPage() {
  const [vehicles, setVehicles] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [plate, setPlate] = useState("");
  const [brand, setBrand] = useState("");
  const [model, setModel] = useState("");
  const [year, setYear] = useState("");
  const [capacity, setCapacity] = useState("");

  const fetchVehicles = async () => {
    try {
      const token = localStorage.getItem("token");
      const res = await axios.get("http://localhost:5243/api/Fleet", {
        headers: { Authorization: `Bearer ${token}` }
      });
      if (Array.isArray(res.data)) setVehicles(res.data);
    } catch (err) {
      // API boş dönerse veya hata olursa arayüz çökmesin
      setVehicles([
        { id: 1, licensePlate: "34 ABC 123", brand: "Mercedes", model: "Actros", modelYear: 2024, capacityInKg: 20000, status: "Aktif" }
      ]);
    }
  };

  useEffect(() => { fetchVehicles(); }, []);

  const handleAddVehicle = (e: React.FormEvent) => {
    e.preventDefault();
    const newVehicle = {
      id: Date.now(),
      licensePlate: plate,
      brand: brand,
      model: model,
      modelYear: Number(year),
      capacityInKg: Number(capacity),
      status: "Aktif"
    };
    setVehicles([newVehicle, ...vehicles]);
    setIsOpen(false);
    setPlate(""); setBrand(""); setModel(""); setYear(""); setCapacity("");
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">🚛 Filo Yönetimi</h1>
          <p className="text-slate-600 text-sm">Sistemde kayıtlı araçların güncel listesi ve durumları.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>+</span> Yeni Araç Ekle
        </button>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 flex justify-between items-center">
          <input type="text" placeholder="Plaka veya Marka ara..." className="px-4 py-2 border rounded-xl w-72 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 bg-slate-50" />
          <span className="text-sm text-slate-500 font-medium">Toplam: {vehicles.length} Araç</span>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Plaka</th>
              <th className="p-4 font-semibold">Marka / Model</th>
              <th className="p-4 font-semibold">Yıl</th>
              <th className="p-4 font-semibold">Kapasite (Kg)</th>
              <th className="p-4 font-semibold">Durum</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {vehicles.length === 0 ? (
              <tr><td colSpan={5} className="p-8 text-center text-slate-400">Sistemde aranan kritere uygun araç bulunamadı.</td></tr>
            ) : (
              vehicles.map((v) => (
                <tr key={v.id} className="hover:bg-slate-50/50 transition-colors">
                  <td className="p-4 font-bold text-slate-900">{v.licensePlate}</td>
                  <td className="p-4">{v.brand} {v.model}</td>
                  <td className="p-4">{v.modelYear}</td>
                  <td className="p-4">{v.capacityInKg} kg</td>
                  <td className="p-4"><span className="px-3 py-1 bg-green-50 text-green-600 rounded-full text-xs font-bold">{v.status}</span></td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {/* Modal Pencere */}
      {isOpen && (
        <div className="fixed inset-0 bg-black/50 z-50 flex items-center justify-center p-4">
          <div className="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6">
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Araç Kaydı</h2>
            <form onSubmit={handleAddVehicle} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Plaka</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={plate} onChange={(e) => setPlate(e.target.value)} required placeholder="34 ABC 34" />
              </div>
              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Marka</label>
                  <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={brand} onChange={(e) => setBrand(e.target.value)} required placeholder="Mercedes" />
                </div>
                <div>
                  <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Model</label>
                  <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={model} onChange={(e) => setModel(e.target.value)} required placeholder="Actros" />
                </div>
              </div>
              <div className="grid grid-cols-2 gap-3">
                <div>
                  <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Model Yılı</label>
                  <input type="number" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={year} onChange={(e) => setYear(e.target.value)} required placeholder="2024" />
                </div>
                <div>
                  <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Kapasite (Kg)</label>
                  <input type="number" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={capacity} onChange={(e) => setCapacity(e.target.value)} required placeholder="15000" />
                </div>
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button type="button" onClick={() => setIsOpen(false)} className="px-4 py-2 border rounded-xl text-sm text-slate-600 hover:bg-slate-50 font-medium">İptal</button>
                <button type="submit" className="px-5 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-xl text-sm font-bold shadow-md">Kaydet</button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
