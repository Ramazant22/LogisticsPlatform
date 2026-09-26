"use client";
import { useEffect, useState } from "react";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import OnboardingModal from "@/components/OnboardingModal";

export default function Dashboard() {
  const [userName, setUserName] = useState("");

  useEffect(() => {
    const token = localStorage.getItem("token");
    const user = localStorage.getItem("user");

    if (!token) {
      window.location.href = "/login";
      return;
    }

    setUserName(user || "Kullanıcı");

    const newConnection = new HubConnectionBuilder()
      .withUrl("http://localhost:5243/trackingHub", {
        accessTokenFactory: () => token
      })
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

    newConnection.start()
      .then(() => console.log("SignalR Başarıyla Bağlandı!"))
      .catch((err) => console.warn("SignalR bağlantısı React Strict Mode nedeniyle yenileniyor."));

    return () => {
      newConnection.stop();
    };
  }, []);

  return (
    <>
      <OnboardingModal />
      <div className="p-8">
        <div className="bg-white rounded-xl shadow-sm p-8 border border-slate-200">
          <h1 className="text-3xl font-bold mb-4 text-slate-800">Hoş Geldin, {userName}</h1>
          <p className="text-slate-600 text-lg">Lojistik platformu ana paneline hoş geldiniz. SignalR canlı bağlantısı aktif. Sol menüden operasyonlarınızı yönetebilirsiniz.</p>
        </div>
      </div>
    </>
  );
}

