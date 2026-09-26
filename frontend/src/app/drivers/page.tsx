"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function DriversPage() {
  const [drivers, setDrivers] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [name, setName] = useState("");
  const [phone, setPhone] = useState("");
  const [license, setLicense] = useState("E Sınıfı");
  const [isLoading, setIsLoading] = useState(false);

  // SQL Veritabanından canlı verileri çek
  const fetchDrivers = async () => {
    try {
      const token = localStorage.getItem("token");
      const res = await axios.get("http://localhost:5243/api/Driver", {
        headers: { Authorization: `Bearer ${token}` }
      });
      setDrivers(res.data);
    } catch (err) {
      console.warn("Veriler çekilemedi:", err);
    }
  };

  useEffect(() => { fetchDrivers(); }, []);

  // Yeni veriyi API (C#) üzerinden SQL'e gönder
  const handleAddDriver = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      const token = localStorage.getItem("token");
      await axios.post("http://localhost:5243/api/Driver", 
        { fullName: name, phoneNumber: phone, licenseType: license },
        { headers: { Authorization: `Bearer ${token}` } }
      );
      
      // Başarılı olursa listeyi API'den tekrar güncelle ve modalı kapat
      await fetchDrivers();
      setIsOpen(false);
      setName(""); setPhone(""); setLicense("E Sınıfı");
    } catch (error) {
      alert("Kayıt sırasında bir hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">👨‍✈️ Sürücü ve Personel Yönetimi</h1>
          <p className="text-slate-600 text-sm">Filodaki araçları kullanacak şoförlerin kayıt ve yetkinlik takibi.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>+</span> Yeni Sürücü Ekle
        </button>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <span className="text-sm text-slate-500 font-medium">Toplam: {drivers.length} Kayıtlı Sürücü (SQL Database)</span>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Ad Soyad</th>
              <th className="p-4 font-semibold">Telefon</th>
              <th className="p-4 font-semibold">Ehliyet Sınıfı</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {drivers.length === 0 ? (
              <tr><td colSpan={3} className="p-8 text-center text-slate-400">Veritabanında kayıtlı sürücü bulunamadı. Yeni bir tane ekleyin!</td></tr>
            ) : (
              drivers.map((d) => (
                <tr key={d.id} className="hover:bg-slate-50/50 transition-colors">
                  <td className="p-4 font-bold text-slate-900">{d.fullName}</td>
                  <td className="p-4">{d.phoneNumber}</td>
                  <td className="p-4"><span className="px-3 py-1 bg-blue-50 text-blue-600 rounded-full text-xs font-bold">{d.licenseType}</span></td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {isOpen && (
        <div className="fixed inset-0 bg-black/50 z-50 flex items-center justify-center p-4">
          <div className="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6">
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Sürücü Kaydı</h2>
            <form onSubmit={handleAddDriver} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Ad Soyad</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={name} onChange={(e) => setName(e.target.value)} required placeholder="Örn: Ahmet Yılmaz" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Telefon Numarası</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={phone} onChange={(e) => setPhone(e.target.value)} required placeholder="0555 444 3322" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Ehliyet Sınıfı</label>
                <select className="w-full px-3 py-2 border rounded-xl text-sm bg-slate-50" value={license} onChange={(e) => setLicense(e.target.value)}>
                  <option value="B Sınıfı">B Sınıfı (Otomobil)</option>
                  <option value="C Sınıfı">C Sınıfı (Kamyon)</option>
                  <option value="E Sınıfı">E Sınıfı (Tır / Ağır Vasıta)</option>
                </select>
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button type="button" onClick={() => setIsOpen(false)} className="px-4 py-2 border rounded-xl text-sm text-slate-600 hover:bg-slate-50 font-medium">İptal</button>
                <button type="submit" disabled={isLoading} className="px-5 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-xl text-sm font-bold shadow-md disabled:opacity-50">
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
