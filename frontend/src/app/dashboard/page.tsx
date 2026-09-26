"use client";

import { useState, useEffect } from "react";
import axios from "axios";
import { LayoutDashboard, Truck, ShoppingCart, DollarSign, Activity, Loader2, ArrowRight } from "lucide-react";
import Link from "next/link";
import toast from "react-hot-toast";

interface ActivityItem {
  id: number;
  action: string;
  detail: string;
  time: string;
  type: "success" | "warning" | "info" | "default";
}

interface DashboardSummary {
  totalVehicles: number;
  activeVehicles: number;
  pendingOrders: number;
  totalRevenue: number;
  recentActivities: ActivityItem[];
}

export default function DashboardPage() {
  const [summary, setSummary] = useState<DashboardSummary | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchSummary = async () => {
      try {
        const response = await axios.get("http://localhost:5243/api/dashboard/summary");
        setSummary(response.data);
      } catch (error) {
        console.error("Dashboard verileri yüklenemedi:", error);
        toast.error("Özet verilere ulaşılamıyor.");
      } finally {
        setLoading(false);
      }
    };

    fetchSummary();
  }, []);

  if (loading) {
    return (
      <div className="flex flex-col items-center justify-center h-[calc(100vh-100px)]">
        <Loader2 className="animate-spin text-blue-600 mb-4" size={40} />
        <p className="text-slate-500 font-medium">Sistem verileri derleniyor...</p>
      </div>
    );
  }

  return (
    <div className="p-8 max-w-7xl mx-auto space-y-8">
      {/* Üst Başlık */}
      <div className="flex justify-between items-end">
        <div>
          <h1 className="text-3xl font-bold text-slate-800 flex items-center gap-3">
            <LayoutDashboard className="text-blue-600" /> Kontrol Merkezi
          </h1>
          <p className="text-slate-500 mt-1">Lojistik ağınızın anlık genel durum özeti.</p>
        </div>
      </div>

      {/* İstatistik Kartları */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        
        {/* Filo Kartı */}
        <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm relative overflow-hidden group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-sm font-semibold text-slate-500 mb-1">Aktif Araçlar</p>
              <h3 className="text-3xl font-bold text-slate-800">{summary?.activeVehicles} <span className="text-sm font-medium text-slate-400">/ {summary?.totalVehicles}</span></h3>
            </div>
            <div className="w-12 h-12 bg-blue-50 text-blue-600 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform">
              <Truck size={24} />
            </div>
          </div>
          <Link href="/fleet" className="text-xs font-bold text-blue-600 flex items-center gap-1 mt-6 hover:gap-2 transition-all">
            Filoya Git <ArrowRight size={14} />
          </Link>
        </div>

        {/* Siparişler Kartı */}
        <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm relative overflow-hidden group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-sm font-semibold text-slate-500 mb-1">Bekleyen Sipariş</p>
              <h3 className="text-3xl font-bold text-slate-800">{summary?.pendingOrders}</h3>
            </div>
            <div className="w-12 h-12 bg-orange-50 text-orange-600 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform">
              <ShoppingCart size={24} />
            </div>
          </div>
          <Link href="/orders" className="text-xs font-bold text-orange-600 flex items-center gap-1 mt-6 hover:gap-2 transition-all">
            Siparişleri Yönet <ArrowRight size={14} />
          </Link>
        </div>

        {/* Finans Kartı */}
        <div className="bg-white p-6 rounded-2xl border border-slate-200 shadow-sm relative overflow-hidden group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-sm font-semibold text-slate-500 mb-1">Aylık Ciro</p>
              <h3 className="text-3xl font-bold text-slate-800">
                {new Intl.NumberFormat('tr-TR', { notation: 'compact', maximumFractionDigits: 1 }).format(summary?.totalRevenue || 0)} ₺
              </h3>
            </div>
            <div className="w-12 h-12 bg-green-50 text-green-600 rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform">
              <DollarSign size={24} />
            </div>
          </div>
          <Link href="/billing" className="text-xs font-bold text-green-600 flex items-center gap-1 mt-6 hover:gap-2 transition-all">
            Finans Paneli <ArrowRight size={14} />
          </Link>
        </div>

        {/* Canlı Takip Kartı */}
        <div className="bg-gradient-to-br from-slate-800 to-slate-900 p-6 rounded-2xl border border-slate-700 shadow-lg relative overflow-hidden group">
          <div className="flex justify-between items-start">
            <div>
              <p className="text-sm font-semibold text-slate-300 mb-1">Sistem Durumu</p>
              <div className="flex items-center gap-2 mt-2">
                <span className="relative flex h-3 w-3">
                  <span className="animate-ping absolute inline-flex h-full w-full rounded-full bg-emerald-400 opacity-75"></span>
                  <span className="relative inline-flex rounded-full h-3 w-3 bg-emerald-500"></span>
                </span>
                <h3 className="text-xl font-bold text-white">Aktif & İzleniyor</h3>
              </div>
            </div>
            <div className="w-12 h-12 bg-slate-700/50 text-white rounded-xl flex items-center justify-center group-hover:scale-110 transition-transform">
              <Activity size={24} />
            </div>
          </div>
          <Link href="/tracking" className="text-xs font-bold text-emerald-400 flex items-center gap-1 mt-6 hover:gap-2 transition-all">
            Haritaya Git <ArrowRight size={14} />
          </Link>
        </div>

      </div>

      {/* Alt Kısım: Son Aktiviteler */}
      <div className="bg-white rounded-2xl border border-slate-200 shadow-sm overflow-hidden">
        <div className="p-6 border-b border-slate-100 flex justify-between items-center bg-slate-50">
          <h2 className="text-lg font-bold text-slate-800">Son Aktiviteler</h2>
          <button className="text-sm font-medium text-blue-600 hover:text-blue-800 transition-colors">
            Tümünü Gör
          </button>
        </div>
        <div className="divide-y divide-slate-100">
          {summary?.recentActivities.map((activity) => (
            <div key={activity.id} className="p-4 flex items-center gap-4 hover:bg-slate-50 transition-colors">
              <div className={`w-2 h-2 rounded-full ${
                activity.type === 'success' ? 'bg-green-500' :
                activity.type === 'warning' ? 'bg-orange-500' :
                activity.type === 'info' ? 'bg-blue-500' : 'bg-slate-300'
              }`}></div>
              <div className="flex-1">
                <p className="text-sm font-bold text-slate-800">{activity.action}</p>
                <p className="text-xs text-slate-500 font-medium">{activity.detail}</p>
              </div>
              <span className="text-xs font-medium text-slate-400 bg-slate-100 px-2 py-1 rounded">
                {activity.time}
              </span>
            </div>
          ))}
        </div>
      </div>

    </div>
  );
}
