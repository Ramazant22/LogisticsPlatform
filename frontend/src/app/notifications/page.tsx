"use client";
import { useState, useEffect } from "react";
import axios from "axios";

export default function NotificationsPage() {
  const [notifications, setNotifications] = useState<any[]>([]);
  const [isOpen, setIsOpen] = useState(false);
  const [recipient, setRecipient] = useState("");
  const [title, setTitle] = useState("");
  const [message, setMessage] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  const fetchNotifications = async () => {
    try {
      const res = await axios.get("http://localhost:5243/api/Notification");
      setNotifications(res.data);
    } catch (err) {
      console.warn("Bildirimler çekilemedi:", err);
    }
  };

  useEffect(() => { fetchNotifications(); }, []);

  const handleSendNotification = async (e: React.FormEvent) => {
    e.preventDefault();
    setIsLoading(true);
    
    try {
      await axios.post("http://localhost:5243/api/Notification", {
        recipient,
        title,
        message
      });
      
      await fetchNotifications();
      setIsOpen(false);
      setRecipient(""); setTitle(""); setMessage("");
    } catch (error) {
      alert("Bildirim gönderilirken bir hata oluştu!");
    } finally {
      setIsLoading(false);
    }
  };

  return (
    <div className="p-8">
      <div className="flex justify-between items-center mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-800">🔔 Bildirim Merkezi</h1>
          <p className="text-slate-600 text-sm">Sistem uyarıları, müşteri bilgilendirmeleri ve sürücü mesajları.</p>
        </div>
        <button 
          onClick={() => setIsOpen(true)}
          className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-2.5 px-4 rounded-xl shadow-md transition-all flex items-center gap-2 text-sm"
        >
          <span>✉️</span> Yeni Bildirim Gönder
        </button>
      </div>

      <div className="grid gap-4">
        {notifications.length === 0 ? (
          <div className="p-8 text-center bg-white rounded-2xl shadow-sm border border-slate-200 text-slate-400">
            Hiç bildirim bulunmuyor.
          </div>
        ) : (
          notifications.map((n) => (
            <div key={n.id} className="bg-white p-5 rounded-2xl shadow-sm border border-slate-200 flex flex-col gap-2 relative overflow-hidden">
              <div className={`absolute top-0 left-0 w-1 h-full ${n.isRead ? 'bg-slate-300' : 'bg-blue-500'}`}></div>
              <div className="flex justify-between items-start pl-2">
                <div>
                  <h3 className="font-bold text-slate-800">{n.title}</h3>
                  <p className="text-xs font-semibold text-slate-500 mt-1">Alıcı: {n.recipient}</p>
                </div>
                <span className="text-xs text-slate-400">{new Date(n.createdAt).toLocaleString()}</span>
              </div>
              <p className="text-sm text-slate-600 pl-2 mt-2">{n.message}</p>
            </div>
          ))
        )}
      </div>

      {isOpen && (
        <div className="fixed inset-0 bg-black/50 z-50 flex items-center justify-center p-4">
          <div className="bg-white rounded-2xl shadow-2xl max-w-md w-full p-6">
            <h2 className="text-xl font-bold text-slate-800 mb-4">Yeni Bildirim Oluştur</h2>
            <form onSubmit={handleSendNotification} className="space-y-4">
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Alıcı</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={recipient} onChange={(e) => setRecipient(e.target.value)} required placeholder="Örn: Sürücü Ahmet, Tüm Müşteriler" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Başlık</label>
                <input type="text" className="w-full px-3 py-2 border rounded-xl text-sm" value={title} onChange={(e) => setTitle(e.target.value)} required placeholder="Güzergah Değişikliği" />
              </div>
              <div>
                <label className="block text-xs font-bold text-slate-600 mb-1 uppercase">Mesaj</label>
                <textarea className="w-full px-3 py-2 border rounded-xl text-sm" rows={4} value={message} onChange={(e) => setMessage(e.target.value)} required placeholder="Bildirim içeriği..."></textarea>
              </div>
              <div className="flex justify-end gap-3 mt-6">
                <button type="button" onClick={() => setIsOpen(false)} className="px-4 py-2 border rounded-xl text-sm text-slate-600 hover:bg-slate-50 font-medium">İptal</button>
                <button type="submit" disabled={isLoading} className="px-5 py-2 bg-blue-600 hover:bg-blue-700 text-white rounded-xl text-sm font-bold disabled:opacity-50">
                  {isLoading ? "Gönderiliyor..." : "Gönder"}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
