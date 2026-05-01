var SweetAlert_custom = {
    init: function () {
      document.querySelector(".sweet-1").onclick = function () {
        Swal.fire(
          'آفرین!',
          'روی دکمه کلیک کردی!',
          'success'
        )
      },
      document.querySelector(".sweet-2").onclick = function () {
        Swal.fire(
          'جادو شد!',
          'از بازدید قالب ادمیرو متشکریم'
          )
      },
      document.querySelector(".sweet-4").onclick = function () {
        Swal.fire(
          'اینترنت؟',
          'هنوز هم وجود داره؟',
          'question'
        )
      },
      (document.querySelector(".sweet-5").onclick = function () {
        Swal.fire({
          title: 'مطمئنی؟',
          text: "بعد از این امکان بازگشت وجود ندارد!",
          icon: 'warning',
          showCancelButton: true,
          confirmButtonColor: '#2e8e87',
          cancelButtonColor: '#C42A02',
          confirmButtonText: 'بله، حذف کن!'
        }).then((result) => {
          if (result.isConfirmed) {
            Swal.fire(
              'حذف شد!',
              'فایل شما حذف شد.',
              'success'
            )
          }
        })
      }),
      document.querySelector(".sweet-6").onclick = function () {
        Swal.fire({
          title: 'انیمیشن سفارشی با Animate.css',
          showClass: {
            popup: 'animate__animated animate__fadeInDown'
          },
          hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
          }
        })
      },
      (document.querySelector(".sweet-7").onclick = function () {
        Swal.fire({
          icon: 'error',
          title: 'اوه...',
          text: 'مشکلی پیش آمد!',
          footer: '<a href="https://sweetalert2.github.io/" target="_blank">چرا این مشکل پیش اومد؟</a>'
        })
      }),
      document.querySelector(".sweet-8").onclick = function () {
        (async () => {
          const { value: email } = await Swal.fire({
            title: 'ایمیل خود را وارد کنید',
            input: 'email',
            inputLabel: 'آدرس ایمیل شما',
            inputPlaceholder: 'آدرس ایمیل خود را وارد کنید'
          })
          if (email) {
            Swal.fire(`ایمیل وارد شده: ${email}`)
          }
          })()
      },
      document.querySelector(".sweet-11").onclick = function () {
        Swal.fire({
          title: 'نام کاربری گیت‌هاب خود را وارد کنید',
          input: 'text',
          inputAttributes: {
            autocapitalize: 'off'
          },
          showCancelButton: true,
          confirmButtonText: 'جستجو',
          showLoaderOnConfirm: true,
          preConfirm: (login) => {
            return fetch(`//api.github.com/users/${login}`)
              .then(response => {
                if (!response.ok) {
                  throw new Error(response.statusText)
                }
                return response.json()
              })
              .catch(error => {
                Swal.showValidationMessage(
                  `درخواست ناموفق بود: ${error}`
                )
              })
          },
          allowOutsideClick: () => !Swal.isLoading()
        }).then((result) => {
          if (result.isConfirmed) {
            Swal.fire({
              title: `آواتار ${result.value.login}`,
              imageUrl: result.value.avatar_url
            })
          }
        })
      },
      document.querySelector('.sweet-12').onclick = function(){
        Swal.fire({
          title: 'می‌خواهید تغییرات ذخیره شوند؟',
          showDenyButton: true,
          showCancelButton: true,
          confirmButtonText: 'ذخیره',
          denyButtonText: `ذخیره نشود`,
        }).then((result) => {
          if (result.isConfirmed) {
            Swal.fire('ذخیره شد!', '', 'success')
          } else if (result.isDenied) {
            Swal.fire('تغییرات ذخیره نشد', '', 'info')
          }
        })
      },
      (document.querySelector(".sweet-13").onclick = function () {
        const Toast = Swal.mixin({
          toast: true,
          position: 'top-end',
          showConfirmButton: false,
          timer: 3000,
          timerProgressBar: true,
          didOpen: (toast) => {
            toast.addEventListener('mouseenter', Swal.stopTimer)
            toast.addEventListener('mouseleave', Swal.resumeTimer)
          }
        })
        Toast.fire({
          icon: 'success',
          title: 'با موفقیت وارد شدید'
        })
      });
      document.querySelector(".sweet-14").onclick = function () {
        Swal.fire({
          title: 'هشدار زمان‌دار',
          text: "صبر کن! تا ۳۰ ثانیه دیگر بسته می‌شوم!",
          showConfirmButton: false,
          timer: 1500
        })
      };
      
      document.querySelector(".sweet-15").onclick = function () {
        Swal.fire({
          title: 'سوال',
          icon: 'question',
          iconHtml: '؟',
          confirmButtonText: 'بله',
          cancelButtonText: 'خیر',
          showCancelButton: true,
          showCloseButton: true
        })
      };
      
      
      document.querySelector(".sweet-16").onclick = function () {
        Swal.fire({
          position: 'top-end',
          icon: 'success',
          title: 'کار شما ذخیره شد',
          showConfirmButton: false,
          timer: 1500
        })
      };
      document.querySelector(".sweet-17").onclick = function () {
        Swal.fire({
          position: 'top-start',
          icon: 'error',
          title: 'مشکلی پیش آمد!',
          showConfirmButton: false,
          timer: 1500
        })
      };
      document.querySelector(".sweet-18").onclick = function () {
        Swal.fire({
          position: 'bottom-start',
          icon: 'info',
          html:
              'می‌توانید استفاده کنید، ' +
              '<a class="font-primary f-18" href="//sweetalert2.github.io" target="_blank">هشدارها</a> ' +
              'و سایر تگ‌های HTML',
          showConfirmButton: false,
        })
      };
      document.querySelector(".sweet-19").onclick = function () {
        Swal.fire({
          position: 'bottom-end',
          icon: 'success',
          title: 'کار شما ذخیره شد',
          showConfirmButton: false,
          timer: 1500
        })
      };
      document.querySelector(".sweet-20").onclick = function () {
        let timerInterval
        Swal.fire({
          title: 'هشدار خودکار!',
          html: 'تا <b></b> میلی‌ثانیه دیگر بسته می‌شوم.',
          timer: 2000,
          timerProgressBar: true,
          didOpen: () => {
            Swal.showLoading()
            const b = Swal.getHtmlContainer().querySelector('b')
            timerInterval = setInterval(() => {
              b.textContent = Swal.getTimerLeft()
            }, 100)
          },
          willClose: () => {
            clearInterval(timerInterval)
          }
        }).then((result) => {
          if (result.dismiss === Swal.DismissReason.timer) {
            console.log('با تایمر بسته شدم')
          }
        })
      };
      document.querySelector(".sweet-21").onclick = function () {
        Swal.fire({
          title: 'عالی!',
          text: 'مدال با تصویر سفارشی.',
          imageUrl: 'https://unsplash.it/400/200',
          imageWidth: 400,
          imageHeight: 200,
          imageAlt: 'تصویر سفارشی',
          showClass: {
            popup: 'animate__animated animate__zoomIn'
          },
        })
      };
      document.querySelector(".sweet-22").onclick = function () {
        (async () => {
          const ipAPI = '//api.ipify.org?format=json'
          const inputValue = fetch(ipAPI)
            .then(response => response.json())
            .then(data => data.ip)
          const { value: ipAddress } = await Swal.fire({
            title: 'آدرس IP خود را وارد کنید',
            input: 'text',
            inputLabel: 'آدرس IP شما',
            inputValue: inputValue,
            showCancelButton: true,
            inputValidator: (value) => {
              if (!value) {
                return 'باید چیزی وارد کنید!'
              }
            }
          })
          if (ipAddress) {
            Swal.fire(`آدرس IP شما: ${ipAddress}`)
          }
          })()
      };
      document.querySelector(".sweet-23").onclick = function () {
        (async () => {
          const { value: url } = await Swal.fire({
            input: 'url',
            inputLabel: 'آدرس URL',
            inputPlaceholder: 'آدرس URL را وارد کنید'
          })
          if (url) {
            Swal.fire(`آدرس وارد شده: ${url}`)
          }
          })()
      };
      document.querySelector(".sweet-24").onclick = function () {
        (async () => {
          const { value: password } = await Swal.fire({
            title: 'رمز عبور خود را وارد کنید',
            input: 'password',
            inputLabel: 'رمز عبور',
            inputPlaceholder: 'رمز عبور خود را وارد کنید',
            inputAttributes: {
              maxlength: 10,
              autocapitalize: 'off',
              autocorrect: 'off'
            }
          })
          if (password) {
            Swal.fire(`رمز وارد شده: ${password}`)
          }
          })()
      };
      document.querySelector(".sweet-25").onclick = function () {
        (async () => {
          const { value: text } = await Swal.fire({
            input: 'textarea',
            inputLabel: 'پیام',
            inputPlaceholder: 'پیام خود را اینجا تایپ کنید...',
            inputAttributes: {
              'aria-label': 'پیام خود را اینجا تایپ کنید'
            },
            showCancelButton: true
          })
          if (text) {
            Swal.fire(text)
          }
          })()
      };
      document.querySelector(".sweet-26").onclick = function () {
        (async () => {
          const { value: fruit } = await Swal.fire({
            title: 'اعتبارسنجی انتخاب',
            input: 'select',
            inputOptions: {
              'میوه‌ها': {
                apples: 'سیب',
                bananas: 'موز',
                grapes: 'انگور',
                oranges: 'پرتقال'
              },
              'سبزیجات': {
                potato: 'سیب‌زمینی',
                broccoli: 'بروکلی',
                carrot: 'هویج'
              },
              'icecream': 'بستنی'
            },
            inputPlaceholder: 'یک میوه انتخاب کنید',
            showCancelButton: true,
            inputValidator: (value) => {
              return new Promise((resolve) => {
                if (value === 'oranges') {
                  resolve()
                } else {
                  resolve('باید پرتقال انتخاب کنید :)')
                }
              })
            }
          })
          if (fruit) {
            Swal.fire(`شما انتخاب کردید: ${fruit}`)
          }
          })()
      };
      document.querySelector(".sweet-27").onclick = function () {
        (async () => {
          const inputOptions = new Promise((resolve) => {
            setTimeout(() => {
              resolve({
                '#ff0000': 'قرمز',
                '#00ff00': 'سبز',
                '#0000ff': 'آبی'
              })
            }, 1000)
          })
          const { value: color } = await Swal.fire({
            title: 'یک رنگ انتخاب کنید',
            input: 'radio',
            inputOptions: inputOptions,
            inputValidator: (value) => {
              if (!value) {
                return 'باید چیزی انتخاب کنید!'
              }
            }
          })
          if (color) {
            Swal.fire({ html: `شما انتخاب کردید: ${color}` })
          }
          })()
      };
      document.querySelector(".sweet-28").onclick = function () {
        (async () => {
          const { value: accept } = await Swal.fire({
            title: 'شرایط و قوانین',
            input: 'checkbox',
            inputValue: 1,
            inputPlaceholder:
              'با شرایط و قوانین موافقم',
            confirmButtonText:
              'ادامه <i class="fa fa-arrow-right"></i>',
            inputValidator: (result) => {
              return !result && 'باید با شرایط و قوانین موافقت کنید'
            }
          })
          if (accept) {
            Swal.fire('با شرایط و قوانین موافقت کردید :)')
          }
          })()
      };
      document.querySelector(".sweet-29").onclick = function () {
        Swal.fire({
          title: 'چند سال داری؟',
          icon: 'question',
          input: 'range',
          inputLabel: 'سن شما',
          inputAttributes: {
            min: 8,
            max: 120,
            step: 1
          },
          inputValue: 25
        })
      };
    },
  };
  SweetAlert_custom.init();
