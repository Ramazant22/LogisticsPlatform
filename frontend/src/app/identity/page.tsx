"use client";

import { useState, useEffect } from "react";
import axios from "axios";
import { ShieldCheck, UserPlus, Search, Loader2, X, Mail, Shield, Clock, Pencil, Save } from "lucide-react";
import toast from "react-hot-toast";

interface User {
  id?: string;
  fullName: string;
  email: string;
  role: string;
  status?: string;
  lastLogin?: string;
}

export default function IdentityPage() {
  const [users, setUsers] = useState<User[]>([]);
  const [loading, setLoading] = useState(true);
  const [searchTerm, setSearchTerm] = useState("");
  
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [editingUser, setEditingUser] = useState<User | null>(null);
  const [editUser, setEditUser] = useState({ fullName: "", email: "", role: "", status: "Active" });
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [newUser, setNewUser] = useState({
    fullName: "",
    email: "",
    role: "Operasyon Sorumlusu"
  });

  useEffect(() => {
    fetchUsers();
  }, []);

  const fetchUsers = async () => {
    try {
      const response = await axios.get("http://localhost:5243/api/identity");
      setUsers(response.data);
    } catch (error) {
      console.error("Kullanıcılar yüklenemedi:", error);
      toast.error("Kullanıcı verileri çekilemedi.");
    } finally {
      setLoading(false);
    }
  };

  const handleAddUser = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsSubmitting(true);
    
    // Gerçek API entegrasyonu simülasyonu
    setTimeout(() => {
      toast.success(`${newUser.fullName} sisteme eklendi ve davet maili gönderildi!`);
      setIsModalOpen(false);
      setNewUser({ fullName: "", email: "", role: "Operasyon Sorumlusu" });
      setIsSubmitting(false);
      setUsers([...users, { ...newUser, id: Math.random().toString(), status: "Active", lastLogin: "Hiç girmedi" }]);
    }, 800);
  };

  const openEditPanel = (user: User) => {
    setEditingUser(user);
    setEditUser({ fullName: user.fullName, email: user.email, role: user.role, status: user.status || "Active" });
  };

  const handleEditUser = (e: React.FormEvent) => {
    e.preventDefault();
    if (!editingUser) return;
    setUsers((currentUsers) => currentUsers.map((user) => user.id === editingUser.id ? { ...user, ...editUser } : user));
    toast.success(`${editUser.fullName} kullanıcısının bilgileri güncellendi.`);
    setEditingUser(null);
  };

  const filteredUsers = users.filter(u => 
    u.fullName.toLowerCase().includes(searchTerm.toLowerCase()) ||
    u.email.toLowerCase().includes(searchTerm.toLowerCase())
  );

  const getRoleBadge = (role: string) => {
    switch (role) {
      case "Sistem Yöneticisi": return <span className="px-2 py-1 bg-purple-100 text-purple-700 rounded-md text-xs font-bold border border-purple-200">{role}</span>;
      case "Operasyon Sorumlusu": return <span className="px-2 py-1 bg-blue-100 text-blue-700 rounded-md text-xs font-bold border border-blue-200">{role}</span>;
      case "Finans Uzmanı": return <span className="px-2 py-1 bg-green-100 text-green-700 rounded-md text-xs font-bold border border-green-200">{role}</span>;
      default: return <span className="px-2 py-1 bg-slate-100 text-slate-700 rounded-md text-xs font-bold border border-slate-200">{role}</span>;
    }
  };

  return (
    <div className="p-8 max-w-7xl mx-auto relative">
      <div className="flex justify-between items-center mb-8">
        <div>
          <h1 className="text-3xl font-bold text-slate-800 flex items-center gap-3">
            <ShieldCheck className="text-blue-600" /> Kimlik ve Yetki Yönetimi
          </h1>
          <p className="text-slate-500 mt-1">Sisteme erişimi olan kullanıcıları, rolleri ve güvenlik ayarlarını yönetin.</p>
        </div>
        <button 
          onClick={() => setIsModalOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white px-4 py-2 rounded-lg flex items-center gap-2 font-medium transition-colors shadow-sm"
        >
          <UserPlus size={20} /> Yeni Kullanıcı
        </button>
      </div>

      <div className="bg-white p-4 rounded-t-xl border border-slate-200 border-b-0 flex justify-between items-center">
        <div className="relative w-72">
          <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" size={18} />
          <input 
            type="text" 
            placeholder="İsim veya E-posta ara..." 
            className="w-full pl-10 pr-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
            value={searchTerm}
            onChange={(e) => setSearchTerm(e.target.value)}
          />
        </div>
        <div className="text-sm text-slate-500 font-medium">
          Toplam: {filteredUsers.length} Kullanıcı
        </div>
      </div>

      <div className="bg-white rounded-b-xl border border-slate-200 shadow-sm overflow-hidden">
        {loading ? (
          <div className="flex flex-col items-center justify-center p-12">
            <Loader2 className="animate-spin text-blue-600 mb-4" size={32} />
            <p className="text-slate-500">Kullanıcı verileri yükleniyor...</p>
          </div>
        ) : (
          <table className="w-full text-left border-collapse">
            <thead>
              <tr className="bg-slate-50 border-b border-slate-200 text-slate-600 text-sm">
                <th className="p-4 font-semibold">Kullanıcı</th>
                <th className="p-4 font-semibold">Rol & Yetki</th>
                <th className="p-4 font-semibold">Durum</th>
                <th className="p-4 font-semibold">Son Giriş</th>
                <th className="p-4 font-semibold text-right">İşlem</th>
              </tr>
            </thead>
            <tbody>
              {filteredUsers.length > 0 ? (
                filteredUsers.map((user, index) => (
                  <tr key={user.id || index} className="border-b border-slate-100 hover:bg-slate-50 transition-colors">
                    <td className="p-4 font-bold text-slate-800 flex items-center gap-3">
                      <div className="w-10 h-10 rounded-full bg-slate-100 border border-slate-200 text-slate-600 flex items-center justify-center font-bold text-sm">
                        {user.fullName.split(' ').map(n => n[0]).join('')}
                      </div>
                      <div className="flex flex-col">
                        <span>{user.fullName}</span>
                        <span className="text-xs text-slate-500 font-medium flex items-center gap-1 mt-0.5"><Mail size={12}/> {user.email}</span>
                      </div>
                    </td>
                    <td className="p-4">
                      <div className="flex items-center gap-2">
                        <Shield size={16} className="text-slate-400"/>
                        {getRoleBadge(user.role)}
                      </div>
                    </td>
                    <td className="p-4">
                      {user.status === "Active" ? (
                        <span className="flex items-center gap-1.5 text-sm font-medium text-green-600"><span className="w-2 h-2 rounded-full bg-green-500"></span> Aktif</span>
                      ) : (
                        <span className="flex items-center gap-1.5 text-sm font-medium text-slate-500"><span className="w-2 h-2 rounded-full bg-slate-400"></span> Pasif</span>
                      )}
                    </td>
                    <td className="p-4 text-slate-500 text-sm font-medium flex items-center gap-1.5 pt-6">
                      <Clock size={14} className="text-slate-400"/> {user.lastLogin}
                    </td>
                    <td className="p-4 text-right">
                      <button type="button" onClick={() => openEditPanel(user)} className="inline-flex items-center gap-2 rounded-lg border border-blue-200 bg-blue-50 px-3 py-2 text-sm font-semibold text-blue-700 transition-colors hover:bg-blue-100 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2" aria-label={`${user.fullName} kullanıcısını düzenle`}>
                        <Pencil size={15} /> Düzenle
                      </button>
                    </td>
                  </tr>
                ))
              ) : (
                <tr>
                  <td colSpan={5} className="p-8 text-center text-slate-500">
                    Sistemde aranan kritere uygun kullanıcı bulunamadı.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        )}
      </div>

      {isModalOpen && (
        <div className="fixed inset-0 bg-slate-900 bg-opacity-50 flex items-center justify-center z-50 backdrop-blur-sm">
          <div className="bg-white rounded-xl shadow-xl w-full max-w-md overflow-hidden animate-in fade-in zoom-in duration-200">
            <div className="flex justify-between items-center p-6 border-b border-slate-100">
              <h3 className="text-xl font-bold text-slate-900">Sisteme Kullanıcı Ekle</h3>
              <button onClick={() => setIsModalOpen(false)} className="text-slate-400 hover:text-slate-600 transition-colors">
                <X size={24} />
              </button>
            </div>
            
            <form onSubmit={handleAddUser} className="p-6 space-y-4">
              <div>
                <label className="block text-sm font-medium text-slate-700 mb-1">Ad Soyad</label>
                <input 
                  type="text" required 
                  className="w-full px-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white placeholder-slate-400 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                  placeholder="Örn: Ayşe Yılmaz"
                  value={newUser.fullName}
                  onChange={(e) => setNewUser({...newUser, fullName: e.target.value})}
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-slate-700 mb-1">E-posta Adresi (Giriş için kullanılacak)</label>
                <input 
                  type="email" required 
                  className="w-full px-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white placeholder-slate-400 focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                  placeholder="ornek@firma.com"
                  value={newUser.email}
                  onChange={(e) => setNewUser({...newUser, email: e.target.value})}
                />
              </div>
              
              <div>
                <label className="block text-sm font-medium text-slate-700 mb-1">Kullanıcı Rolü & Yetkisi</label>
                <select 
                  className="w-full px-4 py-2 border border-slate-200 rounded-lg text-slate-900 bg-white focus:outline-none focus:border-blue-500 focus:ring-1 focus:ring-blue-500"
                  value={newUser.role}
                  onChange={(e) => setNewUser({...newUser, role: e.target.value})}
                >
                  <option value="Sistem Yöneticisi">Sistem Yöneticisi (Tam Yetki)</option>
                  <option value="Operasyon Sorumlusu">Operasyon Sorumlusu</option>
                  <option value="Finans Uzmanı">Finans Uzmanı</option>
                  <option value="Sürücü">Sürücü (Sadece Mobil Uygulama)</option>
                </select>
                <p className="text-xs text-slate-500 mt-2">Kullanıcıya giriş yapabilmesi için otomatik olarak bir davet maili gönderilecektir.</p>
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
                  {isSubmitting && <Loader2 className="animate-spin" size={18} />}
                  Davet Gönder
                </button>
              </div>
            </form>
          </div>
        </div>
      )}

      {editingUser && (
        <div className="fixed inset-0 z-50 flex justify-end bg-slate-900/40 backdrop-blur-sm" onClick={() => setEditingUser(null)}>
          <aside className="h-full w-full max-w-md overflow-y-auto bg-white shadow-2xl" role="dialog" aria-modal="true" aria-labelledby="edit-user-title" onClick={(e) => e.stopPropagation()}>
            <div className="flex items-center justify-between border-b border-slate-200 p-6"><div><p className="text-sm font-medium text-blue-600">Kullanıcı yönetimi</p><h2 id="edit-user-title" className="mt-1 text-xl font-bold text-slate-900">Kullanıcıyı düzenle</h2></div><button type="button" onClick={() => setEditingUser(null)} className="rounded-lg p-2 text-slate-400 hover:bg-slate-100 hover:text-slate-700" aria-label="Paneli kapat"><X size={22} /></button></div>
            <form onSubmit={handleEditUser} className="space-y-5 p-6">
              <div><label className="mb-1 block text-sm font-medium text-slate-700">Ad Soyad</label><input required value={editUser.fullName} onChange={(e) => setEditUser({ ...editUser, fullName: e.target.value })} className="w-full rounded-lg border border-slate-200 bg-white px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" /></div>
              <div><label className="mb-1 block text-sm font-medium text-slate-700">E-posta adresi</label><input required type="email" value={editUser.email} onChange={(e) => setEditUser({ ...editUser, email: e.target.value })} className="w-full rounded-lg border border-slate-200 bg-white px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500" /></div>
              <div><label className="mb-1 block text-sm font-medium text-slate-700">Rol ve yetki</label><select value={editUser.role} onChange={(e) => setEditUser({ ...editUser, role: e.target.value })} className="w-full rounded-lg border border-slate-200 bg-white px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"><option>Sistem Yöneticisi</option><option>Operasyon Sorumlusu</option><option>Finans Uzmanı</option><option>Sürücü</option></select></div>
              <div><label className="mb-1 block text-sm font-medium text-slate-700">Hesap durumu</label><select value={editUser.status} onChange={(e) => setEditUser({ ...editUser, status: e.target.value })} className="w-full rounded-lg border border-slate-200 bg-white px-4 py-2 text-slate-900 focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"><option value="Active">Aktif</option><option value="Inactive">Pasif</option></select></div>
              <div className="flex justify-end gap-3 border-t border-slate-100 pt-5"><button type="button" onClick={() => setEditingUser(null)} className="rounded-lg px-4 py-2 font-medium text-slate-700 hover:bg-slate-100">Vazgeç</button><button type="submit" className="inline-flex items-center gap-2 rounded-lg bg-blue-600 px-4 py-2 font-medium text-white hover:bg-blue-700"><Save size={17} /> Değişiklikleri kaydet</button></div>
            </form>
          </aside>
        </div>
      )}
    </div>
  );
}
