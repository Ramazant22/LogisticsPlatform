"use client";

import React, { useState } from "react";

type User = {
  id: number;
  name: string;
  email: string;
  role: string;
  status: string;
};

export default function UsersPage() {

  const [users] = useState<User[]>([
    {
      id: 1,
      name: "Ramazan Tunç",
      email: "admin@logistics.ai",
      role: "Sistem Yöneticisi",
      status: "Aktif"
    },
    {
      id: 2,
      name: "Ahmet Yılmaz",
      email: "ahmet.y@logistics.ai",
      role: "Operasyon Sorumlusu",
      status: "Aktif"
    }
  ]);

  const [activeModal, setActiveModal] = useState<User | null>(null);

  return (
    <div className="p-8 max-w-7xl mx-auto">

      <h1 className="text-2xl font-bold text-slate-800 mb-6">
        Kimlik ve Yetki Yönetimi
      </h1>

      <div className="bg-white rounded-xl shadow-sm border border-slate-200 p-6">

        <table className="w-full text-left">

          <thead>
            <tr className="border-b text-slate-500 text-sm">

              <th className="pb-3">
                Kullanıcı
              </th>

              <th className="pb-3">
                Rol
              </th>

              <th className="pb-3">
                Durum
              </th>

              <th className="pb-3 text-right">
                Aksiyon
              </th>

            </tr>
          </thead>

          <tbody className="divide-y">

            {users.map((u) => (

              <tr
                key={u.id}
                className="hover:bg-slate-50"
              >

                <td className="py-4 font-bold text-slate-800">

                  {u.name}

                  <span className="block text-xs font-normal text-slate-500">
                    {u.email}
                  </span>

                </td>

                <td className="py-4 text-sm">

                  <span className="px-2 py-1 bg-purple-100 text-purple-700 rounded text-xs font-bold">
                    {u.role}
                  </span>

                </td>

                <td className="py-4 text-sm text-emerald-600 font-semibold">
                  {u.status}
                </td>

                <td className="py-4 text-right">

                  <button
                    onClick={() => setActiveModal(u)}
                    style={{
                      backgroundColor: "#2563eb",
                      color: "#ffffff",
                      padding: "8px 16px",
                      borderRadius: "8px",
                      fontWeight: "bold",
                      border: "none",
                      cursor: "pointer"
                    }}
                  >
                    ✏️ Düzenle
                  </button>

                </td>

              </tr>

            ))}

          </tbody>

        </table>

      </div>


      {/* KULLANICI MODALI */}

      {activeModal && (

        <div
          onClick={() => setActiveModal(null)}
          style={{
            position: "fixed",
            inset: 0,
            backgroundColor: "rgba(0,0,0,0.5)",
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
              borderRadius: "16px",
              width: "420px",
              boxShadow: "0 20px 25px -5px rgba(0,0,0,0.1)"
            }}
          >

            <h2 className="text-xl font-bold mb-6 text-slate-800">
              Kullanıcı Düzenle
            </h2>

            <div className="space-y-4">

              <div>

                <label className="text-xs font-bold text-slate-500">
                  Ad Soyad
                </label>

                <input
                  type="text"
                  defaultValue={activeModal.name}
                  className="w-full p-3 border rounded-lg mt-1"
                />

              </div>

              <div>

                <label className="text-xs font-bold text-slate-500">
                  E-posta
                </label>

                <input
                  type="text"
                  defaultValue={activeModal.email}
                  className="w-full p-3 border rounded-lg mt-1"
                />

              </div>

              <div>

                <label className="text-xs font-bold text-slate-500">
                  Rol
                </label>

                <select
                  defaultValue={activeModal.role}
                  className="w-full p-3 border rounded-lg mt-1"
                >

                  <option>Sistem Yöneticisi</option>
                  <option>Operasyon Sorumlusu</option>
                  <option>Operasyon Kullanıcısı</option>
                  <option>Raporlama Kullanıcısı</option>

                </select>

              </div>

            </div>


            <div className="flex justify-end gap-2 mt-7">

              <button
                onClick={() => setActiveModal(null)}
                className="px-4 py-2 bg-slate-200 rounded-lg font-bold text-slate-700"
              >
                İptal
              </button>

              <button
                onClick={() => {
                  alert("Kullanıcı bilgileri kaydedildi!");
                  setActiveModal(null);
                }}
                className="px-4 py-2 bg-blue-600 text-white rounded-lg font-bold"
              >
                Kaydet
              </button>

            </div>

          </div>

        </div>

      )}

    </div>
  );
}
