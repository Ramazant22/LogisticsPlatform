"use client";
import "./globals.css";
import Sidebar from "@/components/Sidebar";
import { usePathname } from "next/navigation";
import { useEffect, useState } from "react";
import axios from "axios";

export default function RootLayout({ children }: { children: React.ReactNode; }) {
  const pathname = usePathname();
  const [isClient, setIsClient] = useState(false);
  const isAuthPage = pathname === "/login" || pathname === "/register";

  useEffect(() => {
    setIsClient(true);
    const token = localStorage.getItem("token");
    if (!token && !isAuthPage) {
      window.location.href = "/login";
      return;
    }
    if (token) axios.defaults.headers.common.Authorization = `Bearer ${token}`;
  }, [pathname, isAuthPage]);

  return (
    <html lang="tr" className="light" style={{ colorScheme: "light" }}>
      {/* Siyah arkaplanı önlemek için light class'ı eklendi ve inline style uygulandı */}
      <body className="flex min-h-screen bg-slate-50 text-slate-900 !bg-slate-50 !text-slate-900">
        {isClient && !isAuthPage && <Sidebar />}
        <main className="flex-1 overflow-x-hidden overflow-y-auto p-6">
          {children}
        </main>
      </body>
    </html>
  );
}
