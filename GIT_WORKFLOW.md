# آلية العمل المعتمدة

أصبح الفرع الرئيسي `main` هو فرع التسليم الافتراضي للمشروع. يجب أن تكون أي تحديثات مستقبلية موجهة إلى `main` بعد إكمال البناء واختبار الدخان.

## جلب آخر نسخة

```bash
git fetch origin
git switch main
git pull --ff-only origin main
```

## التحقق قبل الدفع

```bash
xbuild SchoolSystem.sln /verbosity:minimal
Tests/verify_architecture.sh
```

إذا نجح البناء والاختبار، تُحفظ التغييرات وتُرفع مباشرة إلى `main`:

```bash
git add -A
git commit -m "وصف التغيير"
git push origin main
```

يجب التأكد بعد الدفع من أن الفرع المحلي والفرع البعيد متطابقان:

```bash
git status --short --branch
git rev-parse HEAD
git rev-parse origin/main
```

لا يتم استخدام `improve-ui-usability-stability` كوجهة تسليم مستقبلية؛ فقد تم دمج محتواه الحالي في `main` وأصبح `main` هو المرجع المعتمد.
