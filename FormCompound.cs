﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LimsProject.BusinessLayer;
using LimsProject.BusinessLayer.Modules;

namespace LimsProject
{
    public partial class FormCompound : LibraryBasicForm.FormBaseEmpty
    {
        public FormCompound()
        {
            InitializeComponent();
        }

        protected override bool Son_Datos_Correctos()
        {
            foreach (CCompound item in gcCompound.DataSource as BindingList<CCompound>)
            {
                if (item.Name_compound.Trim().Length == 0)
                {
                    Comun.Send_message("Compuestos", TypeMsg.error, "Error al ingresar compuesto en fila ");
                    return false;
                }
            }
            return true;
        }

        protected override bool Grabar_Registro()
        {
            try
            {
                // borrar datos
                foreach (CCompound item in new CCompoundFactory().GetAll())
                {
                    item.Status = false;
                    new CCompoundFactory().Update(item);
                }

                // guardar grid
                foreach (CCompound item in gcCompound.DataSource as BindingList<CCompound>)
                {
                    item.Status = true;
                    item.Useredit = Comun.GetUser();
                    item.Dateedit = Comun.GetDate();
                    if (!new CCompoundFactory().Update(item))
                    {
                        item.Usernew = Comun.GetUser();
                        item.Datenew = Comun.GetDate();
                        new CCompoundFactory().Insert(item);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Comun.Send_message("Compuestos", TypeMsg.error, ex.Message);
                return false;
            }            
        }

        protected override void Recuperar_Registro()
        {
            gcCompound.DataSource = new BindingList<CCompound>(new CCompoundFactory().GetAll().Where(x => x.Status == true).ToList());
        }

        protected override void Limpiar_Campos()
        {
            base.Limpiar_Campos();
        }
        
        private void btSave_Click(object sender, EventArgs e)
        {
            if (Son_Datos_Correctos())
                if (Grabar_Registro())
                    MessageBox.Show("Los datos se han guardado correctamente.");
        }

        private void FormCompound_Load(object sender, EventArgs e)
        {
            Recuperar_Registro();
        }

        private void btCancel_Click(object sender, EventArgs e)
        {
            Recuperar_Registro();
        }
    }
}
