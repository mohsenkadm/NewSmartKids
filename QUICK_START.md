# 🚀 دليل البدء السريع - نظام البرومو كود

## ⚡ ابدأ في 3 خطوات فقط!

### الخطوة 1️⃣: تنفيذ SQL Scripts ⚠️ (مهم جداً!)

افتح **SQL Server Management Studio** ونفذ هذه السكريبتات بالترتيب:

```sql
-- 1. تحديث جدول الصور (للفيديو)
-- نفذ: DatabaseScripts\UpdateImagesTable.sql

-- 2. إنشاء جداول البرومو كود
-- نفذ: DatabaseScripts\CreatePromoCodeTables.sql

-- 3. إنشاء الإجراءات المخزنة
-- نفذ: DatabaseScripts\PromoCodeStoredProcedures.sql
```

---

### الخطوة 2️⃣: إضافة الرابط في القائمة

افتح `Views\Shared\_Layout.cshtml` وأضف:

```html
<li class="nav-item">
    <a class="nav-link" href="/Home/PromoCode">
        <i class="material-icons">card_giftcard</i>
        <p>البرومو كود</p>
    </a>
</li>
```

---

### الخطوة 3️⃣: شغّل المشروع

```bash
dotnet run --project WebSmartKid\WebSmartKid.csproj
```

ثم افتح المتصفح:
```
https://localhost:5001/Home/PromoCode
```

---

## ✅ انتهى! الآن يمكنك:

### من لوحة التحكم:
- ✅ إضافة برومو كود جديد
- ✅ تعديل/حذف البرومو كودات
- ✅ تفعيل/إيقاف البرومو كود
- ✅ عرض سجل الاستخدامات

### من التطبيق (API):
```http
POST /PromoCode/UsePromoCode
Content-Type: application/json

{
  "UserId": 1,
  "PromoCodeName": "WELCOME50"
}
```

---

## 📖 للمزيد من التفاصيل:

- **التوثيق الكامل:** `PROMO_CODE_DOCUMENTATION.md`
- **الملخص:** `PROMO_CODE_SUMMARY.md`

---

## 🎉 كل شيء جاهز!

**نظام كامل ومتكامل لإدارة البرومو كود جاهز للاستخدام! 🚀**
