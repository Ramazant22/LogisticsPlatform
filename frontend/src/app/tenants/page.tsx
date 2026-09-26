"use client";

import React, { useState } from "react";

type Tenant = {
  id: string;
  name: string;
  subdomain: string;
  plan: string;
};

export default function TenantsPage() {

  const tenants: Tenant[] = [
    {
      id: "demo-lojistik",
      name: "Demo Lojistik A.Ş.",
      subdomain: "demo.logistics.ai",
      plan: "Enterprise"
    },
    {
      id: "kuzey-kargo",
      name: "Kuzey Kargo",
      subdomain: "kuzey.logistics.ai",
      plan: "Pro Plan"
    }
  ];


  const [activeTenant, setActiveTenant] = useState<Tenant | null>(null);

  const [activeSection, setActiveSection] = useState<string | null>(null);


  const handleAction = (action: string) => {

    setActiveSection(action);

  };


  return (

    <div className="p-8 max-w-7xl mx-auto">

      <h1 className="text-2xl font-bold text-slate-800 mb-6">
        SaaS Firma Yönetimi (Tenancy)
      </h1>


      <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-6">

        <table className="w-full text-left">

          <thead>

            <tr className="border-b text-slate-500 text-sm">

              <th className="pb-3">
                Firma Adı
              </th>

              <th className="pb-3">
                Subdomain
              </th>

              <th className="pb-3">
                Paket
              </th>

              <th className="pb-3 text-right">
                Aksiyon
              </th>

            </tr>

          </thead>


          <tbody className="divide-y">

            {tenants.map((t) => (

              <tr
                key={t.id}
                className="hover:bg-slate-50"
              >

                <td className="py-4 font-bold text-slate-800">
                  {t.name}
                </td>


                <td className="py-4 text-sm text-blue-600">
                  {t.subdomain}
                </td>


                <td className="py-4 text-sm">

                  <span className="px-2 py-1 bg-blue-100 text-blue-700 rounded text-xs font-bold">
                    {t.plan}
                  </span>

                </td>


                <td className="py-4 text-right">

                  {/* ARTIK LINK DEĞİL GERÇEK BUTON */}

                  <button
                    onClick={() => {
                      setActiveTenant(t);
                      setActiveSection(null);
                    }}
                    style={{
                      backgroundColor: "#10b981",
                      color: "#ffffff",
                      padding: "8px 16px",
                      borderRadius: "8px",
                      fontWeight: "bold",
                      border: "none",
                      cursor: "pointer"
                    }}
                  >
                    ⚙️ Yönet
                  </button>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>


      {/* =====================================================
          FİRMA YÖNETİM MODALI
          ===================================================== */}

      {activeTenant && (

        <div
          onClick={() => setActiveTenant(null)}
          style={{
            position: "fixed",
            inset: 0,
            backgroundColor: "rgba(0,0,0,0.55)",
            display: "flex",
            alignItems: "center",
            justifyContent: "center",
            zIndex: 9999
          }}
        >

          <div
            onClick={(e) => e.stopPropagation()}
            style={{
              backgroundColor: "#ffffff",
              padding: "30px",
              borderRadius: "18px",
              width: "520px",
              maxWidth: "90%",
              boxShadow: "0 25px 50px -12px rgba(0,0,0,0.25)"
            }}
          >

            {/* BAŞLIK */}

            <div className="flex justify-between items-start mb-6">

              <div>

                <h2 className="text-xl font-bold text-slate-800">
                  {activeTenant.name}
                </h2>

                <p className="text-sm text-slate-500 mt-1">
                  {activeTenant.subdomain}
                </p>

              </div>


              <button
                onClick={() => setActiveTenant(null)}
                className="text-slate-400 hover:text-slate-700 text-xl"
              >
                ✕
              </button>

            </div>


            {/* YÖNETİM BUTONLARI */}

            <div className="grid grid-cols-2 gap-3">


              {/* Firma Bilgileri */}

              <button
                onClick={() => handleAction("Firma Bilgileri")}
                className="p-4 border rounded-xl text-left hover:bg-slate-50 transition"
              >

                <div className="text-xl mb-1">
                  🏢
                </div>

                <div className="font-bold text-slate-800">
                  Firma Bilgileri
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  Firma bilgilerini yönet
                </div>

              </button>


              {/* Kullanıcılar */}

              <button
                onClick={() => handleAction("Kullanıcı Yönetimi")}
                className="p-4 border rounded-xl text-left hover:bg-slate-50 transition"
              >

                <div className="text-xl mb-1">
                  👥
                </div>

                <div className="font-bold text-slate-800">
                  Kullanıcılar
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  Firma kullanıcılarını yönet
                </div>

              </button>


              {/* Paket */}

              <button
                onClick={() => handleAction("Paket Yönetimi")}
                className="p-4 border rounded-xl text-left hover:bg-slate-50 transition"
              >

                <div className="text-xl mb-1">
                  💳
                </div>

                <div className="font-bold text-slate-800">
                  Paket Yönetimi
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  Abonelik ve paket işlemleri
                </div>

              </button>


              {/* Ayarlar */}

              <button
                onClick={() => handleAction("Firma Ayarları")}
                className="p-4 border rounded-xl text-left hover:bg-slate-50 transition"
              >

                <div className="text-xl mb-1">
                  ⚙️
                </div>

                <div className="font-bold text-slate-800">
                  Firma Ayarları
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  Sistem ayarlarını yönet
                </div>

              </button>


              {/* Yetkilendirme */}

              <button
                onClick={() => handleAction("Yetkilendirme")}
                className="p-4 border rounded-xl text-left hover:bg-slate-50 transition"
              >

                <div className="text-xl mb-1">
                  🔐
                </div>

                <div className="font-bold text-slate-800">
                  Yetkilendirme
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  Roller ve izinler
                </div>

              </button>


              {/* Raporlar */}

              <button
                onClick={() => handleAction("Raporlar")}
                className="p-4 border rounded-xl text-left hover:bg-slate-50 transition"
              >

                <div className="text-xl mb-1">
                  📊
                </div>

                <div className="font-bold text-slate-800">
                  Raporlar
                </div>

                <div className="text-xs text-slate-500 mt-1">
                  Firma kullanım raporları
                </div>

              </button>

            </div>


            {/* SEÇİLEN YÖNETİM İŞLEMİ */}

            {activeSection && (

              <div className="mt-6 p-5 bg-slate-50 rounded-xl border">

                <div className="flex items-center justify-between">

                  <div>

                    <h3 className="font-bold text-slate-800">
                      {activeSection}
                    </h3>

                    <p className="text-sm text-slate-500 mt-1">
                      {activeTenant.name} için bu bölüm yönetilecek.
                    </p>

                  </div>

                  <button
                    onClick={() => {
                      alert(
                        activeSection +
                        " bölümü açılmak üzere."
                      );
                    }}
                    className="px-4 py-2 bg-blue-600 text-white rounded-lg font-bold"
                  >
                    Aç
                  </button>

                </div>

              </div>

            )}


            {/* ALT BUTONLAR */}

            <div className="flex justify-between mt-7 pt-5 border-t">

              <button
                onClick={() => {
                  alert(
                    activeTenant.name +
                    " firması devre dışı bırakılacak."
                  );
                }}
                className="px-4 py-2 text-red-600 border border-red-200 rounded-lg font-bold hover:bg-red-50"
              >
                ⛔ Firmayı Devre Dışı Bırak
              </button>


              <button
                onClick={() => setActiveTenant(null)}
                className="px-4 py-2 bg-slate-200 rounded-lg font-bold text-slate-700"
              >
                Kapat
              </button>

            </div>

          </div>

        </div>

      )}

    </div>

  );
}
