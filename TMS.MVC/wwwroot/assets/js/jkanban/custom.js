"use strict";


"use strict";

// کانبان شماره ۱
var kanban1 = new jKanban({
  element: '#demo1',
  boards: [
    {
      id: '_todo',
      title: 'برای انجام (۲)',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">23/7/22</span>
                    <span class="badge badge-primary f-right">متوسط</span>
                    <h6>طراحی داشبورد</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/6.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">04/7/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/9.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments me-1"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye me-1"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    },
    {
      id: '_doing',
      title: 'در حال انجام (۲)',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">24/7/22</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/1.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments me-1"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye me-1"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">08/2/23</span>
                    <span class="badge badge-success f-right">کم</span>
                    <h6>مشکل داشبورد</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/9.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Pixelstrap, نیویورک</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments me-1"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye me-1"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    },
    {
      id: '_done',
      title: 'انجام شده (۲)',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">04/7/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/1.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments me-1"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye me-1"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">23/7/22</span>
                    <span class="badge badge-primary f-right">متوسط</span>
                    <h6>طراحی داشبورد</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/9.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments me-1"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye me-1"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    }
  ]
});


"use strict";

// کانبان شماره ۲
var kanban2 = new jKanban({
  element: '#demo2',
  gutter: '15px',
  click: function (el) {
    alert(el.innerHTML); // کلیک روی هر کارت، محتوای آن را نمایش می‌دهد
  },
  boards: [
    {
      id: '_todo',
      title: 'برای انجام (فقط در حالت در حال انجام)',
      class: 'bg-info',
      dragTo: ['_working'], // می‌تواند به ستون در حال انجام منتقل شود
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">24/7/22</span>
                    <span class="badge badge-info f-right">متوسط</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/6.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">08/2/23</span>
                    <span class="badge badge-success f-right">کم</span>
                    <h6>مشکل داشبورد</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/6.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Pixelstrap, نیویورک</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    },
    {
      id: '_working',
      title: 'در حال انجام',
      class: 'bg-primary',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">04/7/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/1.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">12/4/23</span>
                    <span class="badge badge-success f-right">کم</span>
                    <h6>مشکل داشبورد</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/1.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Pixelstrap, نیویورک</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    },
    {
      id: '_done',
      title: 'انجام شده (فقط در حالت در حال انجام)',
      class: 'bg-success',
      dragTo: ['_working'], // می‌تواند به ستون در حال انجام منتقل شود
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">24/7/22</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/7.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">04/7/23</span>
                    <span class="badge badge-success f-right">کم</span>
                    <h6>مشکل داشبورد</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/7.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Pixelstrap, نیویورک</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    }
  ]
});

"use strict";

// کانبان شماره ۳
var kanban3 = new jKanban({
  element: '#demo3',
  gutter: '15px',
  click: function (el) {
    alert(el.innerHTML); // کلیک روی هر کارت، محتوای آن را نمایش می‌دهد
  },
  boards: [
    {
      id: '_todo',
      title: 'برای انجام',
      class: 'info',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">08/2/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/7.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">12/4/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/9.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    },
    {
      id: '_working',
      title: 'در حال انجام',
      class: 'warning',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">04/7/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <img class="mt-2 img-fluid" src="./assets/images/other-images/maintenance-bg.jpg" alt="">
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/1.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">04/7/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/6.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    },
    {
      id: '_done',
      title: 'انجام شده',
      class: 'success',
      item: [
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">24/7/22</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/7.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        },
        {
          title: `<a class="kanban-box" href="javascript:void(0)">
                    <span class="date">08/2/23</span>
                    <span class="badge badge-danger f-right">فوری</span>
                    <img class="mt-2 img-fluid" src="./assets/images/other-images/sidebar-bg.jpg" alt="">
                    <h6>تست سایدبار</h6>
                    <div class="d-flex align-items-start">
                      <img class="img-20 me-1 rounded-circle" src="./assets/images/user/6.jpg" alt="">
                      <div class="flex-grow-1">
                        <p class="mb-0">Themeforest, استرالیا</p>
                      </div>
                    </div>
                    <div class="d-flex mt-3">
                      <ul class="list">
                        <li><i class="fa-regular fa-comments"></i>۲</li>
                        <li><i class="fa-solid fa-link"></i>۲</li>
                        <li><i class="fa-solid fa-eye"></i></li>
                      </ul>
                      <div class="customers">
                        <ul>
                          <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                          <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                        </ul>
                      </div>
                    </div>
                  </a>`
        }
      ]
    }
  ]
});
var toDoButton = document.getElementById('addToDo');

toDoButton.addEventListener('click', function () {
  kanban3.addElement('_todo', {
    title: `
      <a class="kanban-box" href="javascript:void(0)">
        <span class="date">04/7/23</span>
        <span class="badge badge-danger f-right">فوری</span>
        <img class="mt-2 img-fluid" src="./assets/images/other-images/sidebar-bg.jpg" alt="">
        <h6>تست سایدبار</h6>
        <div class="d-flex align-items-start">
          <img class="img-20 me-1 rounded-circle" src="./assets/images/user/1.jpg" alt="">
          <div class="flex-grow-1">
            <p class="mb-0">Themeforest, استرالیا</p>
          </div>
        </div>
        <div class="d-flex mt-3">
          <ul class="list">
            <li><i class="fa-regular fa-comments me-1"></i>۲</li>
            <li><i class="fa-solid fa-link"></i>۲</li>
            <li><i class="fa-solid fa-eye me-1"></i></li>
          </ul>
          <div class="customers">
            <ul>
              <li class="d-inline-block me-3"><p class="f-12 mb-0">+۵</p></li>
              <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
              <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
              <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
            </ul>
          </div>
        </div>
      </a>
    `
  });
});

var addBoardDefault = document.getElementById('addDefault');

addBoardDefault.addEventListener('click', function () {
  kanban3.addBoards([
    {
      id: '_default',
      title: 'Kanban Default',
      item: [
        {
          title: `
            <a class="kanban-box" href="javascript:void(0)">
              <span class="date">12/4/23</span>
              <span class="badge badge-danger f-right">Urgent</span>
              <h6>Test Sidebar</h6>
              <div class="d-flex align-items-start">
                <img class="img-20 me-1 rounded-circle" src="./assets/images/user/7.jpg" alt="">
                <div class="flex-grow-1">
                  <p class="mb-0">Themeforest, australia</p>
                </div>
              </div>
              <div class="d-flex mt-3">
                <ul class="list">
                  <li><i class="fa-regular fa-comments me-1"></i>2</li>
                  <li><i class="fa-solid fa-link"></i>2</li>
                  <li><i class="fa-solid fa-eye me-1"></i></li>
                </ul>
                <div class="customers">
                  <ul>
                    <li class="d-inline-block me-3"><p class="f-12 mb-0">+5</p></li>
                    <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                    <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                    <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                  </ul>
                </div>
              </div>
            </a>
          `
        },
        {
          title: `
            <a class="kanban-box" href="javascript:void(0)">
              <span class="date">08/2/23</span>
              <span class="badge badge-danger f-right">Urgent</span>
              <img class="mt-2 img-fluid" src="./assets/images/other-images/maintenance-bg.jpg" alt="">
              <h6>Test Sidebar</h6>
              <div class="d-flex align-items-start">
                <img class="img-20 me-1 rounded-circle" src="./assets/images/user/9.jpg" alt="">
                <div class="flex-grow-1">
                  <p class="mb-0">Themeforest, australia</p>
                </div>
              </div>
              <div class="d-flex mt-3">
                <ul class="list">
                  <li><i class="fa-regular fa-comments me-1"></i>2</li>
                  <li><i class="fa-solid fa-link"></i>2</li>
                  <li><i class="fa-solid fa-eye me-1"></i></li>
                </ul>
                <div class="customers">
                  <ul>
                    <li class="d-inline-block me-3"><p class="f-12 mb-0">+5</p></li>
                    <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/3.jpg" alt=""></li>
                    <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/1.jpg" alt=""></li>
                    <li class="d-inline-block"><img class="img-20 rounded-circle" src="./assets/images/user/5.jpg" alt=""></li>
                  </ul>
                </div>
              </div>
            </a>
          `
        }
      ]
    }
  ]);
});


var removeBoard = document.getElementById('removeBoard');
removeBoard.addEventListener('click', function () {
  kanban3.removeBoard('_done');
});
  