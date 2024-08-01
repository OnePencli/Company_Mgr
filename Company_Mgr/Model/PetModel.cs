using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Company_Mgr.Model
{
    public class PetModel
    {
        private int id;
        private string name;
        private string type;
        private string color;

        [DisplayName("Pet ID")]
        public int Id { get => id; set => id = value; }

        [DisplayName("Pet Name")]
        [Required(ErrorMessage = "宠物名不可以为空！")]
        [StringLength(10,MinimumLength = 2,ErrorMessage = "宠物名长度应为2-10个字符")]
        public string Name { get => name; set => name = value; }

        [DisplayName("Pet Type")]
        [Required(ErrorMessage = "宠物种类不可以为空！")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "宠物种类应为2-10个字符")]
        public string Type { get => type; set => type = value; }

        [DisplayName("Pet Color")]
        [Required(ErrorMessage = "宠物颜色不可以为空！")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "宠物颜色应为2-10个字符")]
        public string Color { get => color; set => color = value; }
    }
}
