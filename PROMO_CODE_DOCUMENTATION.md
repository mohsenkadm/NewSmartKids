# نظام البرومو كود (Promo Code) - دليل كامل

## 📋 نظرة عامة
تم إنشاء نظام كامل لإدارة أكواد الخصم (البرومو كود) مع دعم كامل للواجهة الأمامية والخلفية.

---

## 🗂️ الملفات المنشأة

### 1. Entity Models
- ✅ `WebSmartKid\Models\Entity\PromoCode.cs` - نموذج البرومو كود
- ✅ `WebSmartKid\Models\Entity\UserPromoCode.cs` - نموذج استخدام البرومو كود

### 2. Entity Configuration
- ✅ `WebSmartKid\Models\EntityMap\PromoCodeMap.cs`
- ✅ `WebSmartKid\Models\EntityMap\UserPromoCodeMap.cs`

### 3. Services
- ✅ `WebSmartKid\Helper\Interface\IPromoCodeService.cs` - Interface
- ✅ `WebSmartKid\Helper\Repository\PromoCodeService.cs` - Implementation

### 4. Controllers
- ✅ `WebSmartKid\Controllers\PromoCodeController.cs`

### 5. Views
- ✅ `WebSmartKid\Views\Home\PromoCode.cshtml`
- ✅ `WebSmartKid\wwwroot\js\PromoCode.js`

### 6. Database Scripts
- ✅ `DatabaseScripts\CreatePromoCodeTables.sql` - إنشاء الجداول
- ✅ `DatabaseScripts\PromoCodeStoredProcedures.sql` - الإجراءات المخزنة

### 7. Updated Files
- ✅ `WebSmartKid\Models\Entity\Users.cs` - إضافة AccountBalance
- ✅ `WebSmartKid\Models\Entity\Orders.cs` - إضافة UsedAccountBalance و FinalAmount
- ✅ `WebSmartKid\Models\DB_Context.cs` - إضافة DbSet للبرومو كود

---

## 🔧 خطوات التثبيت

### 1. تنفيذ SQL Scripts (مهم جداً ⚠️)

```bash
# افتح SQL Server Management Studio
# اتصل بقاعدة البيانات
# نفذ السكريبتات بالترتيب التالي:
```

#### أ. إنشاء الجداول
```sql
-- نفذ: DatabaseScripts\CreatePromoCodeTables.sql
```

#### ب. إنشاء الإجراءات المخزنة
```sql
-- نفذ: DatabaseScripts\PromoCodeStoredProcedures.sql
```

### 2. بناء المشروع
```bash
dotnet build WebSmartKid\WebSmartKid.csproj
```

---

## 🎯 الميزات الرئيسية

### ✅ 1. إدارة البرومو كود (لوحة التحكم)
- إضافة برومو كود جديد
- تعديل برومو كود موجود
- حذف برومو كود
- تفعيل/إيقاف البرومو كود
- عرض سجل الاستخدامات
- البحث والتصفية

### ✅ 2. استخدام البرومو كود (التطبيق)
- التحقق من صلاحية البرومو كود
- استخدام البرومو كود مرة واحدة فقط لكل مستخدم
- إضافة المبلغ تلقائياً إلى رصيد المستخدم
- منع استخدام البرومو كود المُوقف

### ✅ 3. رصيد المستخدم
- كل مستخدم لديه رصيد (AccountBalance)
- يزداد الرصيد عند استخدام البرومو كود
- يُستخدم الرصيد عند إنشاء طلب جديد

---

## 📊 البنية التحتية

### جداول قاعدة البيانات

#### 1. PromoCode
```sql
CREATE TABLE [dbo].[PromoCode](
    [PromoCodeId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NVARCHAR(100) NOT NULL,           -- اسم البرومو كود (مثل: WELCOME50)
    [Amount] DECIMAL(18,2) NOT NULL,          -- المبلغ المُراد خصمه
    [IsActive] BIT NOT NULL DEFAULT 1,        -- نشط/غير نشط
    [CreatedDate] DATETIME NOT NULL           -- تاريخ الإنشاء
)
```

#### 2. UserPromoCode (جدول الاستخدام)
```sql
CREATE TABLE [dbo].[UserPromoCode](
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] INT NOT NULL,                    -- معرف المستخدم
    [PromoCodeId] INT NOT NULL,               -- معرف البرومو كود
    [UsedDate] DATETIME NOT NULL,             -- تاريخ الاستخدام
    UNIQUE (UserId, PromoCodeId)              -- ضمان الاستخدام مرة واحدة فقط
)
```

#### 3. Users (تحديث)
```sql
-- إضافة عمود جديد
[AccountBalance] DECIMAL(18,2) NOT NULL DEFAULT 0
```

#### 4. Orders (تحديث)
```sql
-- إضافة أعمدة جديدة
[UsedAccountBalance] DECIMAL(18,2) NOT NULL DEFAULT 0  -- المبلغ المُستخدم من الرصيد
[FinalAmount] DECIMAL(18,2) NOT NULL DEFAULT 0         -- المبلغ النهائي بعد خصم الرصيد
```

---

## 🔌 API Endpoints

### للوحة التحكم (Admin Panel)

#### 1. Get All PromoCode
```http
GET /PromoCode/GetAll?name={name}&isActive={true/false}&index={1}
```

#### 2. Get PromoCode By Id
```http
GET /PromoCode/GetById?id={promoCodeId}
```

#### 3. Create/Update PromoCode
```http
POST /PromoCode/Post
Content-Type: application/json

{
  "PromoCodeId": 0,  // 0 للإضافة، >0 للتعديل
  "Name": "WELCOME50",
  "Amount": 50.00,
  "IsActive": true
}
```

#### 4. Delete PromoCode
```http
DELETE /PromoCode/Delete?id={promoCodeId}
```

#### 5. Toggle Active/Inactive
```http
POST /PromoCode/ToggleActive?id={promoCodeId}&isActive={true/false}
```

#### 6. Get Usage History
```http
GET /PromoCode/GetPromoCodeUsage?promoCodeId={promoCodeId}
```

### للتطبيق (Mobile App)

#### 1. Check if Can Use PromoCode
```http
POST /PromoCode/CanUsePromoCode
Content-Type: application/json

{
  "UserId": 123,
  "PromoCodeName": "WELCOME50"
}

Response:
{
  "success": true,
  "data": {
    "canUse": 1,
    "message": "يمكنك استخدام هذا الكود"
  }
}
```

#### 2. Use PromoCode
```http
POST /PromoCode/UsePromoCode
Content-Type: application/json

{
  "UserId": 123,
  "PromoCodeName": "WELCOME50"
}

Response:
{
  "success": true,
  "data": {
    "success": 1,
    "message": "تم إضافة المبلغ إلى حسابك بنجاح",
    "newBalance": 150.00,
    "amount": 50.00
  }
}
```

---

## 💻 كيفية الاستخدام

### من لوحة التحكم (Admin)

#### 1. إضافة برومو كود جديد
1. اذهب إلى `/Home/PromoCode`
2. اضغط على "إضافة جديد"
3. أدخل:
   - اسم البرومو كود (مثل: WELCOME50)
   - المبلغ (مثل: 50.00)
   - الحالة: نشط/غير نشط
4. اضغط "حفظ"

#### 2. تعديل برومو كود
1. اضغط على "تعديل" بجانب البرومو كود
2. عدّل البيانات
3. اضغط "حفظ"

#### 3. تفعيل/إيقاف برومو كود
- اضغط على "إيقاف" لإيقاف البرومو كود
- اضغط على "تفعيل" لتفعيله مرة أخرى

#### 4. حذف برومو كود
- اضغط على "حذف"
- سيتم حذف البرومو كود وجميع سجلات استخدامه

#### 5. عرض سجل الاستخدامات
- اضغط على الرقم في عمود "عدد الاستخدامات"
- سيظهر جدول بجميع المستخدمين الذين استخدموا هذا الكود

### من التطبيق (Mobile App)

#### 1. التحقق من البرومو كود
```dart
// مثال باستخدام Dart (Flutter)
Future<bool> canUsePromoCode(int userId, String promoCodeName) async {
  final response = await http.post(
    Uri.parse('$baseUrl/PromoCode/CanUsePromoCode'),
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({
      'UserId': userId,
      'PromoCodeName': promoCodeName,
    }),
  );
  
  final data = jsonDecode(response.body);
  return data['data']['canUse'] == 1;
}
```

#### 2. استخدام البرومو كود
```dart
Future<Map<String, dynamic>> usePromoCode(int userId, String promoCodeName) async {
  final response = await http.post(
    Uri.parse('$baseUrl/PromoCode/UsePromoCode'),
    headers: {'Content-Type': 'application/json'},
    body: jsonEncode({
      'UserId': userId,
      'PromoCodeName': promoCodeName,
    }),
  );
  
  final data = jsonDecode(response.body);
  return data['data'];
}
```

---

## 🔄 دورة الحياة الكاملة

### سيناريو: مستخدم يستخدم برومو كود ويقوم بطلب

#### 1. المستخدم يستخدم البرومو كود
```
User Input: WELCOME50
↓
API Call: /PromoCode/UsePromoCode
↓
Validation:
  - هل البرومو كود موجود؟ ✅
  - هل البرومو كود نشط؟ ✅
  - هل المستخدم استخدمه من قبل؟ ❌
↓
Action:
  - إضافة 50 IQD إلى رصيد المستخدم
  - تسجيل الاستخدام في UserPromoCode
↓
Result:
  - رصيد المستخدم: 0 → 50 IQD
```

#### 2. المستخدم يقوم بطلب
```
Order Total: 200 IQD
User Balance: 50 IQD
↓
Calculation:
  - Total: 200 IQD
  - Discount: 20 IQD (10%)
  - NetAmount: 180 IQD
  - UsedAccountBalance: 50 IQD (من الرصيد)
  - FinalAmount: 130 IQD (المبلغ المطلوب دفعه)
↓
Update User Balance:
  - 50 IQD → 0 IQD
↓
Save Order with:
  - Total = 200
  - TotalDiscount = 20
  - NetAmount = 180
  - UsedAccountBalance = 50
  - FinalAmount = 130
```

---

## 🛡️ القواعد والقيود

### 1. قواعد البرومو كود
- ✅ كل برومو كود له اسم فريد
- ✅ يمكن للأدمن تفعيل/إيقاف البرومو كود في أي وقت
- ✅ البرومو كود المُوقف لا يمكن استخدامه
- ✅ يمكن حذف البرومو كود وسيتم حذف سجلات الاستخدام

### 2. قواعد الاستخدام
- ✅ كل مستخدم يمكنه استخدام البرومو كود **مرة واحدة فقط**
- ✅ الرصيد يُضاف فوراً عند الاستخدام
- ✅ لا يمكن استخدام برومو كود مُستخدم مسبقاً
- ✅ لا يمكن استخدام برومو كود غير نشط

### 3. قواعد الرصيد
- ✅ الرصيد لا ينتهي (مدى الحياة)
- ✅ يُستخدم الرصيد تلقائياً عند إنشاء طلب
- ✅ إذا كان الرصيد أكبر من المبلغ المطلوب، يُستخدم المطلوب فقط
- ✅ الرصيد المُستخدم يُخصم من حساب المستخدم

---

## 🧪 اختبار النظام

### 1. اختبار لوحة التحكم
```
✅ 1. إنشاء برومو كود "TEST100" بمبلغ 100 IQD
✅ 2. تعديل المبلغ إلى 150 IQD
✅ 3. إيقاف البرومو كود
✅ 4. محاولة استخدامه من التطبيق (يجب أن يفشل)
✅ 5. تفعيل البرومو كود
✅ 6. استخدامه من التطبيق (يجب أن ينجح)
✅ 7. محاولة استخدامه مرة أخرى (يجب أن يفشل)
✅ 8. عرض سجل الاستخدامات
✅ 9. حذف البرومو كود
```

### 2. اختبار التطبيق
```dart
// Test 1: Check PromoCode
void testCanUsePromoCode() async {
  var result = await canUsePromoCode(1, "WELCOME50");
  print("Can use: $result"); // Should be true for first time
}

// Test 2: Use PromoCode
void testUsePromoCode() async {
  var result = await usePromoCode(1, "WELCOME50");
  print("New Balance: ${result['newBalance']}"); // Should be 50
}

// Test 3: Try to use again
void testUseAgain() async {
  var result = await canUsePromoCode(1, "WELCOME50");
  print("Can use again: $result"); // Should be false
}
```

---

## 📱 إضافة الرابط في القائمة

لإضافة صفحة البرومو كود في قائمة لوحة التحكم، أضف هذا الكود في `_Layout.cshtml`:

```html
<li class="nav-item">
    <a class="nav-link" href="/Home/PromoCode">
        <i class="material-icons">card_giftcard</i>
        <p>البرومو كود</p>
    </a>
</li>
```

---

## ⚠️ ملاحظات مهمة

### 1. الأمان
- ✅ جميع endpoints للأدمن محمية بـ `[Authorize]`
- ✅ endpoints التطبيق مفتوحة لكن تتطلب UserId صحيح
- ✅ الفهرس الفريد يمنع الاستخدام المتكرر

### 2. الأداء
- ✅ استخدام Stored Procedures للعمليات المعقدة
- ✅ Pagination في عرض القوائم
- ✅ Indexes على الأعمدة المستخدمة في البحث

### 3. قاعدة البيانات
- ⚠️ **يجب** تنفيذ SQL Scripts قبل الاستخدام
- ⚠️ تأكد من عمل Backup قبل التنفيذ
- ⚠️ Scripts آمنة ولا تحذف بيانات موجودة

---

## 🎉 الخلاصة

تم إنشاء نظام كامل ومتكامل لإدارة البرومو كود مع:
- ✅ لوحة تحكم كاملة
- ✅ API للتطبيق
- ✅ نظام رصيد للمستخدمين
- ✅ تكامل مع نظام الطلبات
- ✅ أمان وقيود صارمة
- ✅ واجهة سهلة الاستخدام

**الآن جاهز للاستخدام! 🚀**
