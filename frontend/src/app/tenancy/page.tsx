"use client";

import { useState, useEffect } from "react";
import axios from "axios";
import { Server, Globe, Database, Plus, Search, Loader2, X, Building, Settings2, Save } from "lucide-react";
import toast from "react-hot-toast";

interface Tenant {
  id: string;
  name: string;
  subdomain: string;
  plan: string;
  status: string;
  createdAt: string;
  dbStatus: string;
}

export default function TenancyPage() {
  const [tenants, setTenants] = useState<Tenant[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [managingTenant, setManagingTenant] = useState<Tenant | null>(null);
  const [tenantChanges, setTenantChanges] = useState({ name: "", subdomain: "", plan: "Pro", status: "Active", dbStatus: "Healthy" });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [newTenant, setNewTenant] = useState({
    name: "",
    subdomain: "",
    plan: "Pro",
    adminEmail: ""
  });

  useEffect(() => {
    fetchTenants();
  }, []);

  const fetchTenants = async () => {
    try {
      const response = await axios.get("http://localhost:5243/api/tenancy");
      setTenants(response.data);
    } catch (error) {
      console.error("Firmalar yüklenemedi:", error);
      toast.error("Firma verileri çekilemedi.");
    } finally {
      setLoading(false);
    }
  };

  const handleAddTenant = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    
    // Altyapı kurulum simülasyonu
    setTimeout(() => {
      toast.success(`${newTenant.name} için izole veritabanı kuruldu ve sistem aktif edildi!`);
      setIsModalOpen(false);
      setNewTenant({ name: "", subdomain: "", plan: "Pro", adminEmail: "" });
      setIsSubmitting(false);
      setTenants([...tenants, { 
        id: Math.random().toString(), 
        name: newTenant.name, 
        subdomain: `${newTenant.subdomain}.logistics.ai`, 
        plan: newTenant.plan, 
        status: "Active", 
        createdAt: "Şimdi", 
        dbStatus: "Healthy" 
      }]);
    }, 1500);
  };

  const openManagementPanel = (tenant: Tenant) => {
    setManagingTenant(tenant);
    setTenantChanges({ name: tenant.name, subdomain: tenant.subdomain, plan: tenant.plan, status: tenant.status, dbStatus: tenant.dbStatus });
  };

  const handleTenantUpdate = (e: React.FormEvent) => {
    e.preventDefault();
    if (!managingTenant) return;
    setTenants((currentTenants) => currentTenants.map((tenant) => tenant.id === managingTenant.id ? { ...tenant, ...tenantChanges } : tenant));
    toast.success(`${tenantChanges.name} firmasının ayarları güncellendi.`);
    setManagingTenant(null);
  };

  const filteredTenants = tenants.filter(t => 
    t.name.toLowerCase().includes(searchTerm.toLowerCase()) ||
    t.subdomain.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getPlanBadge = (plan: string) => {
    switch (plan) {
      case "Enterprise": return <span className="px-2 py-1 bg-purple-100 text-purple-700 rounded-md text-xs font-bold">Enterprise</span>;
      case "Pro": return <span className="px-2 py-1 bg-blue-100 text-blue-700 rounded-md text-xs font-bold">Pro Plan</span>;
      case "Starter": return <span className="px-2 py-1 bg-slate-100 text-slate-700 rounded-md text-xs font-bold">Starter</span>;
      default: return null;
    }
  };

  return (
    <div className="p-8 max-w-7xl mx-auto relative">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold text-slate-800 flex items-center gap-3">
            <Server className="text-blue-600" /> SaaS Firma Yönetimi (Tenancy)
          </h1>
          <p className="text-slate-500 mt-1">Platformu kullanan lojistik firmalarını, aboneliklerini ve veritabanı durumlarını yönetin.</p>
        </div>
        <button 
          onClick={() => setIsModalOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center gap-2 font-medium transition-colors shadow-sm"
        >
          <Plus size={20} /> Yeni Firma Kaydet
        </button>
      </div>

      <div className="bg-white p-4 rounded-t-xl border border-slate-200 border-b-0 flex justify-between items-center">
        <div className="relative w-72">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" size={18} />
          <input 
            type="text" 
            placeholder="Firma veya Subdomain ara..." 
            className="w-full pl-10 pr-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        <div className="text-sm text-slate-500 font-medium">
          Toplam Kiracı (Tenant): {filteredTenants.length}
        </div>
      </div>

      <div className="bg-white rounded-b-xl border border-slate-200 shadow-sm overflow-hidden">
        {loading ? (
          <div className="flex flex-col items-center justify-center p-12">
            <Loader2 className="animate-spin text-blue-600 mb-4" size={32} />
            <p className="text-slate-500">Tenancy verileri yükleniyor...</p>
          </div>
        ) : (
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-slate-50 border-b border-slate-200 text-slate-600 text-sm">
                <th className="p-4 font-semibold">Firma Adı</th>
                <th className="p-4 font-semibold">Alt Alan Adı (Subdomain)</th>
                <th className="p-4 font-semibold">Paket</th>
                <th className="p-4 font-semibold">Veritabanı Durumu</th>
                <th className="p-4 font-semibold text-right">Aksiyon</th>
              </tr>
            </thead>
            <tbody>
              {filteredTenants.map((tenant, index) => (
                <tr key={tenant.id || index} className="border-b border-slate-100 hover:bg-slate-50 transition-colors">
                  <td className="p-4 font-bold text-slate-800 flex items-center gap-3">
                    <div className="w-8 h-8 rounded bg-blue-50 text-blue-600 flex items-center justify-center">
                      <Building size={16} />
                    </div>
                    {tenant.name}
                  </td>
                  <td className="p-4">
                    <a href={`https://${tenant.subdomain}`} className="text-blue-600 hover:underline flex items-center gap-1.5 font-medium text-sm">
                      <Globe size={14} /> {tenant.subdomain}
                    </a>
                  </td>
                  <td className="p-4">{getPlanBadge(tenant.plan)}</td>
                  <td className="p-4">
                    {tenant.dbStatus === "Healthy" ? (
                      <span className="flex items-center gap-1.5 text-sm font-medium text-green-600"><Database size={14} className="text-green-500"/> İzole DB (Aktif)</span>
                    ) : (
                      <span className="flex items-center gap-1.5 text-sm font-medium text-red-600"><Database size={14} className="text-red-500"/> Bağlantı Hatası</span>
                    )}
                  </td>
                  <td className="p-4 text-right">
                    <button type="button" onClick={() => openManagementPanel(tenant)} className="inline-flex items-center gap-2 rounded-lg border border-blue-200 bg-blue-50 px-3 py-2 text-sm font-semibold text-blue-700 transition-colors hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2" aria-label={`${tenant.name} firmasını yönet`}>
                      <Settings2 size={15} /> Yönet
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>

      {isModalOpen && (
        <div className="fixed inset-0 bg-slate-900 bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-lg overflow-hidden animate-in fade-in zoom-in duration-200">
            <div className="flex justify-between items-center p-6 border-b border-slate-100 bg-slate-50">
              <h3 className="text-xl font-bold text-slate-900 flex items-center gap-2"><Server size={20} className="text-blue-600"/> Yeni Kiracı (Tenant) Kurulumu</h3>
              <button onClick={() => setIsModalOpen(false)} className="text-slate-400 hover:text-slate-600 transition-colors">
                <X size={24} />
              </button>
            </div>
            
            <form onSubmit={handleAddTenant} className="p-6 space-y-4">
              <div>
                <label className="block text-sm font-medium text-slate-700 mb-1">Firma Adı</label>
                <input 
                  type="text" required 
                  className="w-full px-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white placeholder-slate-400 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                  placeholder="Örn: Global Lojistik Ltd."
                  value={newTenant.name}
                  onChange={(e) => setNewTenant({...newTenant, name: e.target.value})}
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-slate-700 mb-1">Alt Alan Adı (Subdomain)</label>
                <div className="flex items-center">
                  <input 
                    type="text" required 
                    className="w-full px-4 py-2 border border-slate-200 rounded-l-lg text-slate-900 bg-white placeholder-slate-400 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                    placeholder="global"
                    value={newTenant.subdomain}
                    onChange={(e) => setNewTenant({...newTenant, subdomain: e.target.value.toLowerCase().replace(/[^a-z0-9]/g, '')})}
                  />
                  <span className="px-4 py-2 bg-slate-100 border border-l-0 border-slate-200 rounded-r-lg text-slate-500 font-medium">.logistics.ai</span>
                </div>
              </div>
              
              <div className="grid grid-cols-2 gap-4">
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Abonelik Paketi</label>
                  <select 
                    className="w-full px-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                    value={newTenant.plan}
                    onChange={(e) => setNewTenant({...newTenant, plan: e.target.value})}
                  >
                    <option value="Starter">Starter (Max 5 Araç)</option>
                    <option value="Pro">Pro (Max 50 Araç)</option>
                    <option value="Enterprise">Enterprise (Sınırsız)</option>
                  </select>
                </div>
                <div>
                  <label className="block text-sm font-medium text-slate-700 mb-1">Yönetici E-posta</label>
                  <input 
                    type="email" required 
                    className="w-full px-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white placeholder-slate-400 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                    placeholder="admin@firma.com"
                    value={newTenant.adminEmail}
                    onChange={(e) => setNewTenant({...newTenant, adminEmail: e.target.value})}
                  />
                </div>
              </div>

              <div className="bg-blue-50 border border-blue-100 p-3 rounded-lg flex items-start gap-3 mt-4">
                <Database className="text-blue-500 mt-0.5" size={18} />
                <p className="text-xs text-blue-800 font-medium leading-relaxed">
                  Kaydet butonuna tıklandığında bu firma için tamamen izole edilmiş yeni bir veritabanı (Migration) oluşturulacak ve altyapı hazırlanacaktır.
                </p>
              </div>
              
              <div className="pt-4 flex justify-end gap-3">
                <button 
                  type="button" 
                  onClick={() => setIsModalOpen(false)}
                  className="px-4 py-2 text-slate-700 font-medium hover:bg-slate-100 rounded-lg transition-colors"
                >
                  İptal
                </button>
                <button 
                  type="submit" 
                  disabled={isSubmitting}
                  className="bg-blue-600 text-white px-5 py-2 rounded-lg font-medium hover:bg-blue-700 transition-colors flex items-center gap-2 disabled:opacity-70"
                >
                  {isSubmitting ? (
                    <><Loader2 className="animate-spin" size={18} /> Altyapı Kuruluyor...</>
                  ) : (
                    <><Server size={18} /> Firmayı Kur</>
                  )}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {managingTenant && (
        <div className="fixed inset-0 z-50 flex justify-end bg-slate-900/40 backdrop-blur-sm" onClick={() => setManagingTenant(null)}>
          <aside className="h-full w-full max-w-lg overflow-y-auto bg-white shadow-2xl" role="dialog" aria-modal="true" aria-labelledby="tenant-panel-title" onClick={(e) => e.stopPropagation()}>
            <div className="flex items-center justify-between border-b border-slate-200 p-6"><div><p className="text-sm font-medium text-blue-600">Firma yönetimi</p><h2 id="tenant-panel-title" className="mt-1 text-xl font-bold text-slate-900">{managingTenant.name}</h2><p className="mt-1 text-sm text-slate-500">Paket, erişim ve altyapı ayarlarını düzenleyin.</p></div><button type="button" onClick={() => setManagingTenant(null)} className="rounded-lg p-2 text-slate-400 hover:bg-slate-100 hover:text-slate-700" aria-label="Paneli kapat"><X size={22} /></button></div>
            <form onSubmit={handleTenantUpdate} className="space-y-5 p-6">
              <div><label className="mb-1 block text-sm font-medium text-slate-700">Firma adı</label><input required value={tenantChanges.name} onChange={(e) => setTenantChanges({ ...tenantChanges, name: e.target.value })} className="w-full rounded-lg border border-slate-200 px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" /></div>
              <div><label className="mb-1 block text-sm font-medium text-slate-700">Alt alan adı</label><input required value={tenantChanges.subdomain} onChange={(e) => setTenantChanges({ ...tenantChanges, subdomain: e.target.value.toLowerCase().replace(/[^a-z0-9.-]/g, "") })} className="w-full rounded-lg border border-slate-200 px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" /></div>
              <div className="grid grid-cols-2 gap-4"><div><label className="mb-1 block text-sm font-medium text-slate-700">Abonelik paketi</label><select value={tenantChanges.plan} onChange={(e) => setTenantChanges({ ...tenantChanges, plan: e.target.value })} className="w-full rounded-lg border border-slate-200 px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"><option value="Starter">Starter</option><option value="Pro">Pro</option><option value="Enterprise">Enterprise</option></select></div><div><label className="mb-1 block text-sm font-medium text-slate-700">Firma durumu</label><select value={tenantChanges.status} onChange={(e) => setTenantChanges({ ...tenantChanges, status: e.target.value })} className="w-full rounded-lg border border-slate-200 px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"><option value="Active">Aktif</option><option value="Inactive">Pasif</option></select></div></div>
              <div><label className="mb-1 block text-sm font-medium text-slate-700">Veritabanı bağlantısı</label><select value={tenantChanges.dbStatus} onChange={(e) => setTenantChanges({ ...tenantChanges, dbStatus: e.target.value })} className="w-full rounded-lg border border-slate-200 px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"><option value="Healthy">Sağlıklı</option><option value="Error">Bağlantı hatası</option></select></div>
              <div className="rounded-lg border border-blue-100 bg-blue-50 p-4 text-sm text-blue-800">Değişiklikleri kaydederek firma paketini, erişim durumunu ve bağlantı görünümünü güncelleyebilirsiniz.</div>
              <div className="flex justify-end gap-3 border-t border-slate-100 pt-5"><button type="button" onClick={() => setManagingTenant(null)} className="rounded-lg px-4 py-2 font-medium text-slate-700 hover:bg-slate-100">Vazgeç</button><button type="submit" className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-4 py-2 font-medium text-white hover:bg-blue-700"><Save size={17} /> Değişiklikleri kaydet</button></div>
            </form>
          </aside>
        </div>
      )}
    </div>
  );
}
