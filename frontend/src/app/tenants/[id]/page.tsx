"use client";
import { useParams } from "next/navigation";

export default function TenantManagePage() {
  const params = useParams();
  
  return (
    <div className="p-8">
      <div className="mb-6">
        <h1 className="text-2xl font-bold text-slate-800">🏢 Firma Yönetimi</h1>
        <p className="text-slate-600 text-sm">Tenant ID: <span className="font-mono bg-slate-100 px-1 rounded">{params.id}</span></p>
      </div>
      
      <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-200">
          <h2 className="font-bold text-lg mb-4 text-slate-800">Abonelik Ayarları</h2>
          <div className="space-y-4">
            <select className="w-full px-3 py-2 border rounded-xl text-sm">
              <option>Standart Paket</option>
              <option>Premium Paket</option>
              <option>Enterprise Paket</option>
            </select>
            <button className="w-full bg-indigo-600 text-white font-bold py-2 rounded-xl text-sm">Güncelle</button>
          </div>
        </div>
        
        <div className="bg-white p-6 rounded-2xl shadow-sm border border-slate-200">
          <h2 className="font-bold text-lg mb-4 text-slate-800">Durum Kontrolü</h2>
          <button className="w-full bg-red-100 hover:bg-red-200 text-red-700 font-bold py-2 rounded-xl text-sm transition-colors">
            Firmayı Askıya Al (Pasif Yap)
          </button>
        </div>
      </div>
    </div>
  );
}
