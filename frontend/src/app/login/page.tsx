"use client";
import { useState } from "react";
import axios from "axios";
import Link from "next/link";
import { useRouter } from "next/navigation";

export default function LoginPage() {
  const router = useRouter();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      const response = await axios.post("http://localhost:5243/api/Auth/login", {
        email,
        password,
      });

      localStorage.setItem("token", response.data.token);
      localStorage.setItem("user", response.data.userFullName || "Kullanıcı");
      router.replace("/dashboard");
    } catch {
      setError("Giriş başarısız. Lütfen bilgilerinizi kontrol edin.");
    }
  };

  return (
    <div className="flex h-screen items-center justify-center bg-slate-800">
      <div className="w-full max-w-md bg-white p-10 rounded-2xl shadow-2xl">
        <h2 className="text-3xl font-extrabold text-center text-blue-600 mb-2">Lojistik Platformu</h2>
        <p className="mb-8 text-center text-sm text-slate-500">Süreçleri yönetmek için hesabınızla giriş yapın.</p>
        {error && <p className="text-red-500 text-sm mb-4 text-center">{error}</p>}
        <form onSubmit={handleLogin}>
          <div className="mb-5">
            <label className="block text-gray-700 text-sm font-bold mb-2">E-posta</label>
            <input
              type="email"
              className="w-full px-4 py-3 border rounded-lg text-gray-900 bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
            />
          </div>
          <div className="mb-6">
            <label className="block text-gray-700 text-sm font-bold mb-2">Şifre</label>
            <input
              type="password"
              className="w-full px-4 py-3 border rounded-lg text-gray-900 bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <button type="submit" className="w-full bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-4 rounded-lg focus:outline-none transition-colors">
            Giriş Yap
          </button>
          <div className="mt-6 text-center border-t pt-4">
            <p className="text-gray-600 text-sm">
              Hesabınız yok mu? Firma yöneticinizden kullanıcı hesabı isteyin. <br />
              <Link href="/register" className="text-blue-600 hover:text-blue-800 font-bold hover:underline mt-1 inline-block">
                Hesap bilgisi
              </Link>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
}
