import Link from "next/link";

export default function RegisterPage() {
  return (
    <main className="flex min-h-screen items-center justify-center bg-slate-800 p-6">
      <section className="w-full max-w-md rounded-2xl bg-white p-10 text-center shadow-2xl">
        <h1 className="text-3xl font-extrabold text-blue-600">Kullanıcı Hesapları</h1>
        <p className="mt-5 text-slate-600">Kullanıcı hesapları firma yöneticiniz tarafından oluşturulur. Firma adıyla kayıt olmanıza gerek yoktur.</p>
        <p className="mt-3 text-sm text-slate-500">Hesabınız oluşturulduysa e-posta adresiniz ve şifrenizle giriş yapabilirsiniz.</p>
        <Link href="/login" className="mt-8 inline-flex w-full items-center justify-center rounded-lg bg-blue-600 px-4 py-3 font-bold text-white transition-colors hover:bg-blue-700">Giriş Yap</Link>
      </section>
    </main>
  );
}
