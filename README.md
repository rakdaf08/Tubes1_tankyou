# Tubes1_tankyou

## **Penjelasan**

Kami mengimplementasikan **empat strategi greedy** dalam bot yang dibuat:

### **KuncirRamBOT (Bot Utama)**
Bot ini mengimplementasi strategi Greedy by Energy and Distance. Strategi ini mempertimbangkan dua faktor utama: **energi bot** dan **jarak ke musuh**.
- Jika energi bot tinggi, bot akan menyerang dengan agresif dan menyesuaikan kekuatan tembakan berdasarkan jarak.
- Jika energi bot rendah, bot akan lebih fokus menghindar dengan bergerak cepat untuk memperpanjang durasi bertahan.

### **Sakbot**
Bot ini mengimplementasi strategi Greedy by Damage and Accuracy. Strategi ini mengutamakan **damage maksimum dengan akurasi tinggi**.
- Bot akan maju mendekati musuh untuk memperbesar peluang tembakan mengenai target.
- Saat musuh dalam jarak optimal, bot akan menembak dengan kekuatan penuh untuk memaksimalkan damage.

### **Tang**
Bot ini mengimplementasi strategi Greedy by Distance and Avoidance. Strategi ini fokus pada **pemilihan musuh terdekat dan menghindari serangan**.
- Bot akan memindai arena dan memilih musuh dengan jarak paling dekat untuk diserang.
- Jika bot dalam bahaya, bot akan berusaha menghindari tembakan dengan bergerak menjauh.

### **Hytam**
Bot ini mengimplementasi strategi Greedy by Evasion and Opportunistic Attack. Strategi ini mengutamakan **penghindaran serangan musuh sambil menyerang ketika ada kesempatan**.
- Bot akan terus bergerak dalam pola acak atau melingkar untuk menghindari serangan musuh.
- Jika musuh terdeteksi dalam jangkauan, bot akan menembak dengan kekuatan berdasarkan jarak.

---

## **Requirement Program dan Instalasi**

### **1. Requirement**
- **OS:** Windows/Linux/MacOS
- **Runtime:** .NET SDK (versi terbaru) → [Download di sini](https://dotnet.microsoft.com/en-us/download)
- **Robocode Tank Royale Game Engine** → [Download di sini](https://robocode-dev.github.io/tank-royale/)
- **Compiler:** C# (.NET)

### **2. Instalasi**
1. **Pastikan .NET SDK terinstal** dengan menjalankan perintah:
   ```sh
   dotnet --version
   ```
   jika tidak terinstall, download dan install dari link diatas.

2. **Clone Repository ini:**
    ```sh
    git clone https://github.com/rakdaf08/Tubes1_tankyou.git
    cd Tubes1_tankyou
    ```
3. **Pastikan Robocode Tank Royale telah diinstal dan berjalan.**
    Jika belum, terdapat aplikasi robocode di folder engine

## **Cara Menjalankan Program**
### **1. Compile Bot**
1. Buka folder directory bot
   ```
   cd src/KuncirRamBOT
   ```
   atau
   ```
   cd src/alternative-bots/nama-salah-satu-bot
   ```
2. Jalankan .cmd bot
   ```
   ./nama-bot
   ```
   atau .sh jika menggunakan MacBook
   ```
   ./nama-bot.sh
   ```
### **2. Jalankan Game**
1. **Jalankan game engine:**
    ```
    java -jar robocode-tankroyale-gui-0.30.0.jar
    ```
2. Tekan tombol **config** di menu atas, lalu tekan **Bot root directories**
<img width="491" alt="Screenshot 2025-03-24 at 16 47 15" src="https://github.com/user-attachments/assets/cfe404de-2da1-4b27-a510-7c202beb3a10" />

3. Masukkan directories yang berisi bot-bot yang ingin dipakai (src) dan (src/alternative-bots)
4. Tekan tombol **Battle** lalu lakukan **booting** bot yang ingin ditandingkan
<img width="417" alt="Screenshot 2025-03-24 at 16 52 00" src="https://github.com/user-attachments/assets/0e91c8d8-91b1-4b8d-a8df-4a5050d2c94c" />

5. Jika sudah di-boot, bot akan muncul di kotak kiri bawah. Add bot yang ingin ditandingkan
<img width="417" alt="Screenshot 2025-03-24 at 16 53 36" src="https://github.com/user-attachments/assets/2373b44e-6fc4-41c6-8730-a492ea85b969" />

6. Tekan **Start Battle** dan permainan sudah bisa dimulai.

## **Author**
**Nama Kelompok**: tankyou

**Anggota**:
1. Raka Daffa Iftikhaar (13523018)
2. Julius Arthur (13523030)
3. Sakti Bimasena (13523053)



