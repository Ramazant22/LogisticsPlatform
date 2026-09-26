"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function CustomersPage() {
  const [customers, setCustomers] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [phone, setPhone] = useState("");
  const [address, setAddress] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const fetchCustomers = async () => {
    try {
      const res = await axios.get("http://localhost:5243/api/Customer");
      setCustomers(res.data);
    } catch (err) {
      console.warn("Müşteriler çekilemedi:", err);
    }
  };

  useEffect(() => { fetchCustomers(); }, []);

  const handleAddCustomer = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      await axios.post("http://localhost:5243/api/Customer", {
        name,
        email,
        phone,
        address
      });
      
      await fetchCustomers();
      setIsOpen(false);
      setName(""); setEmail(""); setPhone(""); setAddress("");
    } catch (error) {
      alert("Müşteri kaydedilirken bir hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">🏢 Müşteri (CRM) Yönetimi</h1>
          <p className="text-slate-600 text-sm">Lojistik hizmeti alan kurumsal müşterilerin iletişim ve adres bilgileri.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>+</span> Yeni Müşteri Ekle
        </button>
      </div>

      <div className="bg-white rounded-2xl shadow-sm border border-slate-200 overflow-hidden">
        <div className="p-4 border-b border-slate-100 bg-slate-50">
          <span className="text-sm text-slate-500 font-medium">Toplam: {customers.length} Kayıtlı Müşteri (SQL Database)</span>
        </div>
        <table className="w-full text-left border-collapse">
          <thead>
            <tr className="bg-slate-50 text-slate-400 text-xs uppercase tracking-wider border-b border-slate-100">
              <th className="p-4 font-semibold">Müşteri / Şirket Adı</th>
              <th className="p-4 font-semibold">E-Posta</th>
              <th className="p-4 font-semibold">Telefon</th>
              <th className="p-4 font-semibold">Adres</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-slate-100 text-sm text-slate-700">
            {customers.length === 0 ? (
              <tr><td colSpan={4} className="p-8 text-center text-slate-400">Veritabanında kayıtlı müşteri bulunamadı.</td></tr>
            ) : (
              customers.map((c) => (
                <tr key={c.id} className="hover:bg-slate-50/50 transition-colors">
                  <td className="p-4 font-bold text-slate-900">{c.name}</td>
                  <td className="p-4 text-slate-600">{c.email}</td>
                  <td className="p-4">{c.phone}</td>
                  <td className="p-4 text-slate-500">{c.address}</td>
                </tr>
              ))
            )}
          </tbody>
        </table>
      </div>

      {isOpen && (
        <div className="fixed inset-0 bg-black/50 z-50 flex items-center justify-center p-4">
          <div className="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6">
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Müşteri Kaydı</h2>
            <form onSubmit={handleAddCustomer} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Müşteri / Şirket Adı</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={name} onChange={(e) => setName(e.target.value)} required placeholder="Global Lojistik A.Ş." />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">E-Posta</label>
                <input type="email" className="w-full px-3 py-2 border rounded-xl text-sm" value={email} onChange={(e) => setEmail(e.target.value)} required placeholder="info@globallogistics.com" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Telefon</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={phone} onChange={(e) => setPhone(e.target.value)} required placeholder="0216 555 4433" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Adres</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={address} onChange={(e) => setAddress(e.target.value)} required placeholder="İkitelli OSB İstanbul" />
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
