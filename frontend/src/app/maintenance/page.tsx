"use client";

import { useEffect, useMemo, useState } from "react";
import axios from "axios";
import { AlertTriangle, CheckCircle2, Loader2, RefreshCw, Wrench } from "lucide-react";
import toast from "react-hot-toast";

type Vehicle = { id: string; licensePlate: string; brand: string; model: string; currentMileage: number; status: string };

function getMaintenanceState(mileage: number) {
  if (mileage >= 150000) return { label: "Acil bakım", className: "bg-red-50 text-red-700 border-red-200", icon: AlertTriangle };
  if (mileage >= 100000) return { label: "Planlanmalı", className: "bg-amber-50 text-amber-700 border-amber-200", icon: AlertTriangle };
  return { label: "İzleniyor", className: "bg-emerald-50 text-emerald-700 border-emerald-200", icon: CheckCircle2 };
}

export default function MaintenancePage() {
  const [vehicles, setVehicles] = useState<Vehicle[]>([]);
  const [isLoading, setIsLoading] = useState(true);
  const [triggeringId, setTriggeringId] = useState<string | null>(null);

  const loadVehicles = async () => {
    setIsLoading(true);
    try {
      const response = await axios.get<Vehicle[]>("http://localhost:5243/api/Fleet");
      setVehicles(response.data);
    } catch {
      toast.error("Araç verileri yüklenemedi.");
    } finally {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    const loadInitialVehicles = async () => {
      try {
        const response = await axios.get<Vehicle[]>("http://localhost:5243/api/Fleet");
        setVehicles(response.data);
      } catch {
        toast.error("Araç verileri yüklenemedi.");
      } finally {
        setIsLoading(false);
      }
    };

    void loadInitialVehicles();
  }, []);

  const counts = useMemo(() => vehicles.reduce((result, vehicle) => {
    const label = getMaintenanceState(vehicle.currentMileage || 0).label;
    if (label === "Acil bakım") result.urgent += 1;
    if (label === "Planlanmalı") result.planned += 1;
    return result;
  }, { urgent: 0, planned: 0 }), [vehicles]);

  const notifyMaintenance = async (vehicle: Vehicle) => {
    setTriggeringId(vehicle.id);
    try {
      await axios.post("http://localhost:5243/api/Maintenance/trigger", { vehicleId: vehicle.id, plateNumber: vehicle.licensePlate });
      toast.success(`${vehicle.licensePlate} için bakım bildirimi oluşturuldu.`);
    } catch {
      toast.error("Bakım bildirimi oluşturulamadı.");
    } finally {
      setTriggeringId(null);
    }
  };

  return <div className="mx-auto max-w-7xl space-y-6 p-8">
    <div className="flex flex-col justify-between gap-4 sm:flex-row sm:items-end"><div><p className="flex items-center gap-2 text-sm font-semibold text-orange-600"><Wrench size={17} /> Bakım Yönetimi</p><h1 className="mt-1 text-3xl font-bold text-slate-900">Filo bakım takibi</h1><p className="mt-2 text-slate-500">Araçların kilometre bilgisini takip edin ve bakım bildirimlerini operasyon ekibine iletin.</p></div><button type="button" onClick={() => void loadVehicles()} disabled={isLoading} className="inline-flex items-center gap-2 rounded-lg border border-slate-200 bg-white px-4 py-2.5 text-sm font-semibold text-slate-700 shadow-sm transition hover:bg-slate-50 disabled:opacity-50"><RefreshCw size={16} className={isLoading ? "animate-spin" : ""} /> Yenile</button></div>
    <div className="grid gap-4 md:grid-cols-3"><div className="rounded-2xl border border-slate-200 bg-white p-5 shadow-sm"><p className="text-sm font-medium text-slate-500">Toplam araç</p><p className="mt-2 text-3xl font-bold text-slate-900">{vehicles.length}</p></div><div className="rounded-2xl border border-amber-100 bg-amber-50 p-5"><p className="text-sm font-medium text-amber-700">Planlanması gereken</p><p className="mt-2 text-3xl font-bold text-amber-900">{counts.planned}</p></div><div className="rounded-2xl border border-red-100 bg-red-50 p-5"><p className="text-sm font-medium text-red-700">Acil bakım</p><p className="mt-2 text-3xl font-bold text-red-900">{counts.urgent}</p></div></div>
    <section className="overflow-hidden rounded-2xl border border-slate-200 bg-white shadow-sm"><div className="flex items-center justify-between border-b border-slate-100 bg-slate-50 px-6 py-4"><div><h2 className="font-semibold text-slate-900">Bakım durumu</h2><p className="mt-1 text-xs text-slate-500">Durum, kaydedilmiş araç kilometresi üzerinden gösterilir.</p></div></div>{isLoading ? <div className="flex items-center justify-center gap-3 p-12 text-slate-500"><Loader2 className="animate-spin text-blue-600" size={24} /> Araçlar yükleniyor…</div> : vehicles.length === 0 ? <div className="p-12 text-center text-slate-500">Bakım takibi için önce filo ekranından araç ekleyin.</div> : <div className="overflow-x-auto"><table className="w-full text-left text-sm"><thead className="border-b border-slate-200 text-xs uppercase tracking-wide text-slate-500"><tr><th className="px-6 py-4">Araç</th><th className="px-6 py-4">Kilometre</th><th className="px-6 py-4">Durum</th><th className="px-6 py-4 text-right">İşlem</th></tr></thead><tbody className="divide-y divide-slate-100">{vehicles.map((vehicle) => { const state = getMaintenanceState(vehicle.currentMileage || 0); const Icon = state.icon; return <tr key={vehicle.id} className="hover:bg-slate-50"><td className="px-6 py-4"><p className="font-semibold text-slate-800">{vehicle.licensePlate}</p><p className="mt-1 text-xs text-slate-500">{vehicle.brand} {vehicle.model}</p></td><td className="px-6 py-4 font-medium text-slate-700">{(vehicle.currentMileage || 0).toLocaleString("tr-TR")} km</td><td className="px-6 py-4"><span className={`inline-flex items-center gap-1.5 rounded-full border px-2.5 py-1 text-xs font-semibold ${state.className}`}><Icon size={14} /> {state.label}</span></td><td className="px-6 py-4 text-right"><button type="button" onClick={() => void notifyMaintenance(vehicle)} disabled={triggeringId === vehicle.id} className="inline-flex items-center gap-2 rounded-lg bg-orange-600 px-3 py-2 text-xs font-semibold text-white hover:bg-orange-700 disabled:opacity-50">{triggeringId === vehicle.id ? <Loader2 className="animate-spin" size={15} /> : <Wrench size={15} />} Bakım bildirimi</button></td></tr>; })}</tbody></table></div>}</section>
  </div>;
}
