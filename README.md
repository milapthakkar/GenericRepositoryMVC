# GenericRepositoryMVC
Generic Repository Using Unit of work For MVC and Web API

# How To Use

    public class DepartmentsController : Controller
    {
        private UnitOfWork _unitofwork = null;

        public DepartmentsController()
        {
            _unitofwork = new UnitOfWork(new ApplicationDBContext());
        }
        public DepartmentsController(UnitOfWork unitofwork)
        {
            this._unitofwork = unitofwork;
        }

        // GET: Departments
        public ActionResult Index()
        {
            return View(_unitofwork.GetRepository<Department>().GetAll().ToList());
        }
        
        // GET: Departments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _unitofwork.GetRepository<Department>().Get(x => x.DepartmentID == id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // GET: Departments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DepartmentID,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                var repository = _unitofwork.GetRepository<Department>();
                repository.Add(department);
                _unitofwork.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(department);
        }

        // GET: Departments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _unitofwork.GetRepository<Department>().Get(x => x.DepartmentID == id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DepartmentID,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {
                _unitofwork.GetRepository<Department>().Update(department);
                _unitofwork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(department);
        }

        // GET: Departments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Department department = _unitofwork.GetRepository<Department>().Get(x => x.DepartmentID == id);
            if (department == null)
            {
                return HttpNotFound();
            }
            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {           
            Department department = _unitofwork.GetRepository<Department>().Get(x => x.DepartmentID == id);
            _unitofwork.GetRepository<Department>().Delete(department);
            _unitofwork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _unitofwork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
