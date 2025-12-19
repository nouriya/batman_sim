# شبیه ساز بتمن در گاتهام

## Introduction

این پروژه یک شبیه ساز مدل walking simulator البته با ماشین از روی بازی بتمن هست.
هدف اصلی کار پیاده سازی حالات stealth،normal و Alert است.
کنترل ماشین batmobile به عهدا ی بازیکن است و با استفاده از دکمه های مناسب که در پایین توضیح داده شده است میتوانید سرعت، جهت حرکت و حالت های بازی را مدیریت کنید.

---

## Project Overview

سه دسته کد اصلی داریم :

1. **BatmanController.cs**
    این script حرکت اتوموبیل یا بتموبیل را مدیریت می‌کند.


2. **SceneManager.cs**
   حالت های مختلف بازی (stealth,normal,alert)
   همراه با تاثیرشان در سرعت بازیکن ، صدای آلارم و آزیر و نور های موجود در بازی در این قسمت مدیریت می‌شوند.

3. **CameraOrbitFollow.cs**
    هرگاه بازیکن جابجا بشود دوربین نیز همراه او باید برود، سرعت و زاوین و جهت حرکت دوربین در این قسمت پیاده سازی شده.
---

## Controls

در جدول زیر کنترل های بازی را میتوانید مشاهده کنید.
| Action                 | Key                             | Description                                                |
| ---------------------- | ------------------------------- | ---------------------------------------------------------- |
| Move Forward/Backward  | W / S or ↑ / ↓                  | Move Batman forward or backward                            |
| Turn Left/Right        | A / D or ← / →                  | Rotate Batman                                              |
| Sprint / Boost         | Left Shift                      | Increase movement speed temporarily                        |
| Switch to Normal Mode  | N                               | Set state to **Normal**                                    |
| Switch to Stealth Mode | C                               | Set state to **Stealth** (reduced speed and dimmed lights) |
| Switch to Alert Mode   | Space                           | Set state to **Alert** (flashing lights and alarm)         |
| Toggle Bat-Signal      | B                               | Enable or disable Bat-Signal rotation in the sky           |
| Orbit Camera           | Right Mouse Button + Move Mouse | Rotate camera around Batman                                |
| Reset Camera           | Programmatic                    | Camera automatically follows behind the player             |

---

## Game States

### 1. Normal Mode(حالت عادی)

* حالت عادی است.
* سرعت و نور کنترل بازی در حالت عادی قرار دارد و آزیر خطر نیز نداریم.

### 2. Stealth Mode

* با دکمه  **C** فعال می‌شود.
* سرعت کاهش می‌یابد.
* نور زمینه هم کم می‌شود.

### 3. Alert Mode(حالت هشدار)

* با **Space** فعال می‌شود .
* نور افزایش پیدا می‌کند.
* Activates **flashing red/blue lights**.
* صدای **آلارم** پخش می‌شود.

---

## Bat-Signal

*   سیگنال بتمن در آسمان ظاهر می‌شود.
*   باید برای فعال و غیر فعال کردنش دکه **b** را بزنیم.
---
## نکات نهایی
*  پروژه سه بعدی پیاده سازی شده.
*  ویدیوی بازی را اگر بشود در فایل زیپ همراه با لینک ارسال می‌کنم.

---


