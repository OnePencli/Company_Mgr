using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Company_Mgr.Model;
using Company_Mgr.Views;

namespace Company_Mgr.Presents
{
    public class PetPresenter
    {
        private IPetView view;    //这是和显示界面负责交互
        private IPetRepository repository;   //这里相当于在和数据进行交互
        private BindingSource petBindingSource;   //这里相当于一个箱子  把从数据库中读取的数据放在这里面  随时进行调用 
        private IEnumerable<PetModel> petList;
        public PetPresenter(IPetView view, IPetRepository repository)
        {
            this.petBindingSource = new BindingSource();
            this.view = view;
            this.repository = repository;
            //这里是对事件进行绑定
            this.view.SearchEvent += SearchPet;
            this.view.AddNewEvent += AddNewPet;
            this.view.EditEvent += LoadSelectedPetToEdit;
            this.view.DeleteEvent += DeleteSelectedPetToEdit;
            this.view.SaveEvent += SavePet;
            this.view.CancelEvent += CancelAction;
            this.view.SetPetListBindingSource(petBindingSource);
            LoadAllPetList();
            this.view.Show();
        }
        private void LoadAllPetList()
        {
            petList = repository.GetAll();
            petBindingSource.DataSource = petList; 
        }
        private void SearchPet(object sender, EventArgs e)
        {
            //这里去判断View层次  的SearchValue属性是否为空  
            //因为在PetView.cs中  它实现了IPetView 将里面的属性SearchValue和其搜索框的文本内容绑定
            //这里相当于就在判断搜索框内容  然后传输数据源
            if (!string.IsNullOrWhiteSpace(this.view.SearchValue))
                petList = repository.GetByValue(this.view.SearchValue);
            else
                petList = repository.GetAll();
            petBindingSource.DataSource = petList;
        }
        private void CancelAction(object sender, EventArgs e)
        {
            CleanviewFields();
        }
        private void DeleteSelectedPetToEdit(object sender, EventArgs e)
        {
            try
            {
                var pet = (PetModel)petBindingSource.Current;
                repository.Delete(pet.Id);
                view.IsSuccessful = true;
                view.Message = "删除成功！";
                LoadAllPetList();
            }
            catch (Exception ex)
            {
                view.IsSuccessful = false;
                view.Message = "删除失败！";
            }
        }
        private void SavePet(object sender, EventArgs e)
        {
            var model = new PetModel();
            model.Id = Convert.ToInt32(view.PetID);
            model.Name = view.PetName;
            model.Type = view.PetType;
            model.Color = view.PetColor;
            try
            {
                new Common.ModelDataVaildation().Validate(model);
                if(view.IsEdit)
                {
                    repository.Edit(model);
                    view.Message = "宠物编辑成功！";
                }
                else
                {
                    repository.Add(model);
                    view.Message = "宠物添加成功！";
                }
                view.IsSuccessful = true;
                LoadAllPetList();
                CleanviewFields();
            }
            catch (Exception ex)
            {
                view.IsSuccessful = false;
                view.Message = ex.Message;
            }
        }
        private void CleanviewFields()
        {
            view.PetID = "0";
            view.PetColor = "";
            view.PetName = "";
            view.PetType = "";
        }
        private void LoadSelectedPetToEdit(object sender, EventArgs e)
        {
            var pet = (PetModel)petBindingSource.Current;
            view.PetID = pet.Id.ToString();
            view.PetColor = pet.Color.ToString();
            view.PetName = pet.Name.ToString();
            view.PetType = pet.Type.ToString();
            view.IsEdit = true;
        }
        private void AddNewPet(object sender, EventArgs e)
        {
            view.IsEdit = false;
        }
    }
}
