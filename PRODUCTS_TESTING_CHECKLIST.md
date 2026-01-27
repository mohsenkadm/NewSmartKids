# Products Module - Testing Checklist

## 🧪 Quick Testing Guide

### Prerequisites
- Ensure database is seeded with test data
- Clear browser cache
- Open browser console to check for errors
- Have test images ready (JPG, PNG)

---

## 1. ✅ SAVE Operation Testing

### Test 1.1: Create New Product with Uploaded Images
**Steps:**
1. Click "اضافة جديد" button
2. Fill in all required fields:
   - Name: "منتج اختبار 1"
   - Detail: "تفاصيل المنتج"
   - Price: "99.99"
   - Count: "10"
   - Category: Select any
3. Upload 2-3 images using file input
4. Click "حفظ"

**Expected Result:**
- ✅ Success notification appears
- ✅ Modal closes automatically
- ✅ Product appears in table
- ✅ Form is cleared

---

### Test 1.2: Create New Product with Image URL
**Steps:**
1. Click "اضافة جديد" button
2. Fill in required fields
3. Enter image URL: `https://via.placeholder.com/150`
4. Click "حفظ"

**Expected Result:**
- ✅ Success notification
- ✅ Product saved with external image
- ✅ Modal closes

---

### Test 1.3: Create Product with Both Upload and URL
**Steps:**
1. Fill form
2. Upload 1 image file
3. Enter image URL
4. Click "حفظ"

**Expected Result:**
- ✅ Both images saved
- ✅ Can view both in "عرض الصور"

---

### Test 1.4: Validation Errors
**Steps:**
1. Click "اضافة جديد"
2. Leave Name field empty
3. Click "حفظ"

**Expected Result:**
- ❌ Error: "رجاءا ادخل الاسم"
- ✅ Modal stays open
- ✅ Other fields keep their values

**Repeat for:**
- Empty Detail: "رجاءا ادخل التفاصيل"
- Empty Price: "رجاءا ادخل السعر بشكل صحيح"
- No Category: "رجاءا اختر الصنف"

---

## 2. ✅ EDIT Operation Testing

### Test 2.1: Edit Existing Product
**Steps:**
1. Click "تعديل" on any product
2. Confirm the warning dialog
3. Verify form is populated with product data
4. Change Name to "منتج معدل"
5. Upload new image
6. Click "حفظ"

**Expected Result:**
- ✅ Confirmation dialog appears first
- ✅ Form populated correctly
- ✅ Success notification
- ✅ Modal closes
- ✅ Table updates with new data
- ✅ Age table is reset

---

### Test 2.2: Edit with URL Field
**Steps:**
1. Edit product
2. Add or change URL field
3. Save

**Expected Result:**
- ✅ URL saved correctly
- ✅ Shows in table

---

### Test 2.3: Edit Discount Settings
**Steps:**
1. Edit product
2. Check "خصم" checkbox
3. Set discount percentage to "20"
4. Save

**Expected Result:**
- ✅ Checkbox state saved
- ✅ Discount percentage displayed correctly

---

## 3. ✅ DELETE Operation Testing

### Test 3.1: Delete Product
**Steps:**
1. Click "حذف" on a product
2. Click "OK" on confirmation
3. Wait for response

**Expected Result:**
- ✅ Confirmation dialog appears
- ✅ Success notification
- ✅ Product removed from table
- ✅ Page refreshes automatically
- ✅ All product images deleted from database

---

### Test 3.2: Cancel Delete
**Steps:**
1. Click "حذف"
2. Click "Cancel" on confirmation

**Expected Result:**
- ✅ Nothing happens
- ✅ Product remains in table

---

### Test 3.3: Delete Product Image
**Steps:**
1. Click "عرض الصور" on product with multiple images
2. Click "حذف" on one image
3. Confirm deletion

**Expected Result:**
- ✅ Confirmation dialog
- ✅ Success notification
- ✅ Image removed from list
- ✅ Table refreshes automatically
- ✅ Other images remain

---

## 4. ✅ SEARCH Operation Testing

### Test 4.1: Search by Name
**Steps:**
1. Enter product name in "بحث عن الاسم"
2. Click "بحث"

**Expected Result:**
- ✅ Only matching products shown
- ✅ Table updates quickly
- ✅ Page index resets to 1

---

### Test 4.2: Filter by Category
**Steps:**
1. Select category from dropdown
2. Click "بحث"

**Expected Result:**
- ✅ Only products in that category shown
- ✅ Table updates

---

### Test 4.3: Combined Search
**Steps:**
1. Enter name AND select category
2. Click "بحث"

**Expected Result:**
- ✅ Products matching BOTH criteria shown

---

### Test 4.4: Filter Products Without Images
**Steps:**
1. Check "المنتجات التي لا تحتوي على صور"
2. Click "بحث"

**Expected Result:**
- ✅ Only products with no images shown
- ✅ Products with images hidden

---

### Test 4.5: Pagination
**Steps:**
1. Perform search
2. Click "Next" (»)
3. Click "Previous" («)

**Expected Result:**
- ✅ Page number increases/decreases
- ✅ Different products shown
- ✅ Cannot go below page 1
- ✅ Search filters maintained across pages

---

### Test 4.6: Clear Search
**Steps:**
1. Search for something
2. Clear search fields
3. Click "بحث"

**Expected Result:**
- ✅ All products shown again

---

## 5. ✅ AGE Management Testing

### Test 5.1: View Product Ages
**Steps:**
1. Click "العمر" on any product
2. View age list

**Expected Result:**
- ✅ Modal opens
- ✅ Age list displays
- ✅ Checkboxes show current state

---

### Test 5.2: Toggle Single Age
**Steps:**
1. Open age modal
2. Check/uncheck an age
3. Close and reopen modal

**Expected Result:**
- ✅ State saved immediately
- ✅ State persists after close/reopen

---

### Test 5.3: Select All Ages
**Steps:**
1. Open age modal
2. Click "تحديد الكل"
3. Verify all ages checked
4. Click again to uncheck all

**Expected Result:**
- ✅ All ages checked/unchecked
- ✅ State saved in database

---

## 6. ✅ IMAGE Management Testing

### Test 6.1: View Product Images
**Steps:**
1. Click "عرض الصور"
2. View images

**Expected Result:**
- ✅ Modal opens
- ✅ All images display (max 50x50)
- ✅ Images load with lazy loading
- ✅ Delete button for each image

---

### Test 6.2: No Images State
**Steps:**
1. Create product without images
2. Click "عرض الصور"

**Expected Result:**
- ✅ Shows "لا توجد صور"

---

## 7. ⚡ PERFORMANCE Testing

### Test 7.1: Large Dataset
**Steps:**
1. Load page with 100+ products
2. Observe load time
3. Scroll through table
4. Perform search

**Expected Result:**
- ✅ Page loads in < 2 seconds
- ✅ Smooth scrolling
- ✅ Search responds quickly

---

### Test 7.2: Multiple Rapid Operations
**Steps:**
1. Click search multiple times rapidly
2. Open/close modals quickly
3. Navigate pagination fast

**Expected Result:**
- ✅ No errors in console
- ✅ No frozen UI
- ✅ Operations complete correctly

---

### Test 7.3: Memory Usage
**Steps:**
1. Open Browser DevTools → Performance
2. Record while performing all operations
3. Check memory usage

**Expected Result:**
- ✅ No memory leaks
- ✅ Event listeners cleaned up
- ✅ Stable memory usage

---

## 8. 🔒 SECURITY Testing

### Test 8.1: XSS Prevention
**Steps:**
1. Create product with name: `<script>alert('XSS')</script>`
2. Save and view in table

**Expected Result:**
- ✅ Script NOT executed
- ✅ Displays as plain text: `<script>alert('XSS')</script>`

---

### Test 8.2: SQL Injection Prevention (Backend)
**Steps:**
1. Search for: `' OR '1'='1`
2. Enter product name: `'; DROP TABLE Products; --`

**Expected Result:**
- ✅ Treated as literal string
- ✅ No database damage
- ✅ Safe search results

---

## 9. 🌐 BROWSER Compatibility

Test on:
- [ ] Chrome (latest)
- [ ] Firefox (latest)
- [ ] Edge (latest)
- [ ] Safari (if available)
- [ ] Mobile browsers

---

## 10. 📱 RESPONSIVE Testing

### Test 10.1: Mobile View
**Steps:**
1. Open DevTools → Toggle device toolbar
2. Select mobile device
3. Test all operations

**Expected Result:**
- ✅ Buttons readable
- ✅ Modals fit screen
- ✅ Table scrollable
- ✅ All functions work

---

## 🐛 Error Scenarios

### Test E1: Network Error
**Steps:**
1. Open DevTools → Network tab
2. Set to "Offline"
3. Try to save product

**Expected Result:**
- ✅ Error notification shows
- ✅ No crash
- ✅ Can retry when online

---

### Test E2: Invalid Image URL
**Steps:**
1. Enter invalid URL: `invalid-url`
2. Save product

**Expected Result:**
- ✅ Backend validates URL
- ✅ Appropriate error message

---

### Test E3: Large File Upload
**Steps:**
1. Try to upload 10MB+ image
2. Save

**Expected Result:**
- ✅ Backend handles file size limit
- ✅ Error message if too large

---

## ✅ FINAL CHECKLIST

Before marking as complete:

- [ ] All 30+ tests passed
- [ ] No console errors
- [ ] No memory leaks
- [ ] All browsers tested
- [ ] Mobile responsive
- [ ] Accessibility checked
- [ ] Performance acceptable
- [ ] Security verified

---

## 🚨 Known Issues / Notes

_Document any issues found during testing:_

1. Issue: _______________
   Status: _______________
   Priority: _______________

2. Issue: _______________
   Status: _______________
   Priority: _______________

---

## 📊 Test Results Summary

| Category | Tests | Passed | Failed | Notes |
|----------|-------|--------|--------|-------|
| Save | 4 | | | |
| Edit | 3 | | | |
| Delete | 3 | | | |
| Search | 6 | | | |
| Age Mgmt | 3 | | | |
| Images | 2 | | | |
| Performance | 3 | | | |
| Security | 2 | | | |
| Browser | 5 | | | |
| Responsive | 1 | | | |
| Errors | 3 | | | |
| **TOTAL** | **35** | **__** | **__** | |

---

**Tested By:** _______________  
**Date:** _______________  
**Environment:** _______________  
**Notes:** _______________
