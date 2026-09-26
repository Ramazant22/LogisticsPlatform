"use client";
import { useState, useEffect } from "react";

export default function OnboardingModal() {
  const [isVisible, setIsVisible] = useState(false);

  useEffect(() => {
    // Kullanıcı bu turu daha önce tamamlamış mı kontrol et
    const hasSeen = localStorage.getItem("hasSeenOnboarding");
    if (!hasSeen) {
      setIsVisible(true);
    }
  }, []);

  const handleFinish = () => {
    localStorage.setItem("hasSeenOnboarding", "true");
    setIsVisible(false);
  };

  if (!isVisible) return null;

  return (
    <div className="fixed inset-0 bg-black bg-opacity-50 z-50 flex items-center justify-center p-4">
      <div className="bg-white rounded-xl shadow-2xl max-w-2xl w-full overflow-hidden animate-fade-in-up">
        <div className="bg-blue-600 p-6 text-white text-center">
          <h2 className="text-3xl font-bold">Lojistik Platformuna Hoş Geldiniz! 🚀</h2>
          <p className="mt-2 text-blue-100">Platformun yeteneklerini kısaca keşfedelim.</p>
        </div>
        <div className="p-6 space-y-4 text-gray-700">
          <div className="flex items-start gap-4 p-3 bg-gray-50 rounded-lg">
            <span className="text-2xl">📦</span>
            <div>
              <h3 className="font-bold text-lg">Siparişler (Orders)</h3>
              <p className="text-sm">Tüm lojistik gönderilerinizi oluşturun, durumlarını güncelleyin ve geçmişi takip edin.</p>
            </div>
          </div>
          <div className="flex items-start gap-4 p-3 bg-gray-50 rounded-lg">
            <span className="text-2xl">🚛</span>
            <div>
              <h3 className="font-bold text-lg">Filo Yönetimi (Fleet)</h3>
              <p className="text-sm">Şirketinize ait araçları ekleyin, bakım bildirimlerini ve kapasite durumlarını yönetin.</p>
            </div>
          </div>
          <div className="flex items-start gap-4 p-3 bg-gray-50 rounded-lg">
            <span className="text-2xl">📍</span>
            <div>
              <h3 className="font-bold text-lg">Canlı Takip (Tracking)</h3>
              <p className="text-sm">SignalR altyapısı sayesinde sevkiyatların konumlarını harita üzerinde gerçek zamanlı olarak izleyin.</p>
            </div>
          </div>
        </div>
        <div className="p-6 bg-gray-50 border-t flex justify-end">
          <button 
            onClick={handleFinish}
            className="bg-blue-600 hover:bg-blue-700 text-white font-bold py-3 px-8 rounded-lg transition-all"
          >
            Anladım, Platformu Kullanmaya Başla
          </button>
        </div>
      </div>
    </div>
  );
}
