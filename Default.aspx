<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StudentManagementSystem.Default" %>

<!DOCTYPE html>
<html lang="ar" dir="rtl">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>نظام إدارة الطلاب</title>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0/css/all.min.css" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css2?family=Cairo:wght@300;400;600;700&display=swap" rel="stylesheet">
    <link href="Styles/main.css" rel="stylesheet">
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- Header -->
            <header class="header">
                <div class="header-content">
                    <h1><i class="fas fa-graduation-cap"></i> نظام إدارة الطلاب</h1>
                    <p>نظام متكامل لإدارة بيانات الطلاب</p>
                </div>
            </header>

            <!-- Navigation -->
            <nav class="nav-tabs">
                <button type="button" class="nav-tab active" onclick="showTab('dashboard')">
                    <i class="fas fa-home"></i> الرئيسية
                </button>
                <button type="button" class="nav-tab" onclick="showTab('add-student')">
                    <i class="fas fa-user-plus"></i> إضافة طالب
                </button>
                <button type="button" class="nav-tab" onclick="showTab('view-students')">
                    <i class="fas fa-users"></i> عرض الطلاب
                </button>
                <button type="button" class="nav-tab" onclick="showTab('search')">
                    <i class="fas fa-search"></i> البحث
                </button>
            </nav>

            <!-- Dashboard Tab -->
            <div id="dashboard" class="tab-content active">
                <div class="dashboard-grid">
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-users"></i>
                        </div>
                        <div class="stat-info">
                            <h3 id="totalStudents">0</h3>
                            <p>إجمالي الطلاب</p>
                        </div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-user-check"></i>
                        </div>
                        <div class="stat-info">
                            <h3 id="activeStudents">0</h3>
                            <p>الطلاب النشطين</p>
                        </div>
                    </div>
                    <div class="stat-card">
                        <div class="stat-icon">
                            <i class="fas fa-graduation-cap"></i>
                        </div>
                        <div class="stat-info">
                            <h3 id="graduatedStudents">0</h3>
                            <p>الخريجين</p>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Add Student Tab -->
            <div id="add-student" class="tab-content">
                <div class="form-container">
                    <h2><i class="fas fa-user-plus"></i> إضافة طالب جديد</h2>
                    <div class="form-grid">
                        <div class="form-group">
                            <label for="txtStudentName">الاسم الكامل *</label>
                            <asp:TextBox ID="txtStudentName" runat="server" CssClass="form-control" placeholder="أدخل الاسم الكامل"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStudentName" runat="server" 
                                ControlToValidate="txtStudentName" 
                                ErrorMessage="الاسم مطلوب" 
                                CssClass="error-message" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtStudentID">رقم الطالب *</label>
                            <asp:TextBox ID="txtStudentID" runat="server" CssClass="form-control" placeholder="أدخل رقم الطالب"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStudentID" runat="server" 
                                ControlToValidate="txtStudentID" 
                                ErrorMessage="رقم الطالب مطلوب" 
                                CssClass="error-message" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtEmail">البريد الإلكتروني</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="أدخل البريد الإلكتروني"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                ControlToValidate="txtEmail" 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" 
                                ErrorMessage="البريد الإلكتروني غير صحيح" 
                                CssClass="error-message" 
                                Display="Dynamic">
                            </asp:RegularExpressionValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtPhone">رقم الهاتف</label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="أدخل رقم الهاتف"></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label for="ddlDepartment">القسم *</label>
                            <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">اختر القسم</asp:ListItem>
                                <asp:ListItem Value="علوم الحاسوب">علوم الحاسوب</asp:ListItem>
                                <asp:ListItem Value="هندسة البرمجيات">هندسة البرمجيات</asp:ListItem>
                                <asp:ListItem Value="تقنية المعلومات">تقنية المعلومات</asp:ListItem>
                                <asp:ListItem Value="الأعمال">الأعمال</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvDepartment" runat="server" 
                                ControlToValidate="ddlDepartment" 
                                ErrorMessage="القسم مطلوب" 
                                CssClass="error-message" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtGPA">المعدل التراكمي</label>
                            <asp:TextBox ID="txtGPA" runat="server" CssClass="form-control" placeholder="أدخل المعدل التراكمي"></asp:TextBox>
                            <asp:RangeValidator ID="rvGPA" runat="server" 
                                ControlToValidate="txtGPA" 
                                MinimumValue="0" 
                                MaximumValue="4" 
                                Type="Double" 
                                ErrorMessage="المعدل يجب أن يكون بين 0 و 4" 
                                CssClass="error-message" 
                                Display="Dynamic">
                            </asp:RangeValidator>
                        </div>

                        <div class="form-group">
                            <label for="ddlStatus">الحالة *</label>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">اختر الحالة</asp:ListItem>
                                <asp:ListItem Value="نشط">نشط</asp:ListItem>
                                <asp:ListItem Value="خريج">خريج</asp:ListItem>
                                <asp:ListItem Value="منقطع">منقطع</asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvStatus" runat="server" 
                                ControlToValidate="ddlStatus" 
                                ErrorMessage="الحالة مطلوبة" 
                                CssClass="error-message" 
                                Display="Dynamic">
                            </asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group">
                            <label for="txtEnrollmentDate">تاريخ التسجيل</label>
                            <asp:TextBox ID="txtEnrollmentDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-actions">
                        <asp:Button ID="btnAddStudent" runat="server" Text="إضافة الطالب" 
                            CssClass="btn btn-primary" OnClick="btnAddStudent_Click" />
                        <asp:Button ID="btnClear" runat="server" Text="مسح النموذج" 
                            CssClass="btn btn-secondary" OnClick="btnClear_Click" CausesValidation="false" />
                    </div>

                    <asp:Label ID="lblMessage" runat="server" CssClass="message"></asp:Label>
                </div>
            </div>

            <!-- View Students Tab -->
            <div id="view-students" class="tab-content">
                <div class="students-container">
                    <h2><i class="fas fa-users"></i> قائمة الطلاب</h2>
                    
                    <div class="table-controls">
                        <div class="search-box">
                            <i class="fas fa-search"></i>
                            <asp:TextBox ID="txtSearch" runat="server" CssClass="search-input" 
                                placeholder="البحث في الطلاب..." OnTextChanged="txtSearch_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                        <div class="filter-controls">
                            <asp:DropDownList ID="ddlFilterDepartment" runat="server" CssClass="filter-select" 
                                OnSelectedIndexChanged="ddlFilterDepartment_SelectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Value="">جميع الأقسام</asp:ListItem>
                                <asp:ListItem Value="علوم الحاسوب">علوم الحاسوب</asp:ListItem>
                                <asp:ListItem Value="هندسة البرمجيات">هندسة البرمجيات</asp:ListItem>
                                <asp:ListItem Value="تقنية المعلومات">تقنية المعلومات</asp:ListItem>
                                <asp:ListItem Value="الأعمال">الأعمال</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="table-responsive">
                        <asp:GridView ID="gvStudents" runat="server" CssClass="students-table" 
                            AutoGenerateColumns="False" OnRowCommand="gvStudents_RowCommand" 
                            OnRowDataBound="gvStudents_RowDataBound" AllowPaging="True" 
                            PageSize="10" OnPageIndexChanging="gvStudents_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="StudentID" HeaderText="رقم الطالب" />
                                <asp:BoundField DataField="StudentName" HeaderText="الاسم الكامل" />
                                <asp:BoundField DataField="Email" HeaderText="البريد الإلكتروني" />
                                <asp:BoundField DataField="Phone" HeaderText="رقم الهاتف" />
                                <asp:BoundField DataField="Department" HeaderText="القسم" />
                                <asp:BoundField DataField="GPA" HeaderText="المعدل" DataFormatString="{0:F2}" />
                                <asp:BoundField DataField="Status" HeaderText="الحالة" />
                                <asp:BoundField DataField="EnrollmentDate" HeaderText="تاريخ التسجيل" DataFormatString="{0:yyyy-MM-dd}" />
                                <asp:TemplateField HeaderText="الإجراءات">
                                    <ItemTemplate>
                                        <asp:Button ID="btnEdit" runat="server" Text="تعديل" 
                                            CommandName="EditStudent" CommandArgument='<%# Eval("StudentID") %>' 
                                            CssClass="btn btn-sm btn-warning" />
                                        <asp:Button ID="btnDelete" runat="server" Text="حذف" 
                                            CommandName="DeleteStudent" CommandArgument='<%# Eval("StudentID") %>' 
                                            CssClass="btn btn-sm btn-danger" 
                                            OnClientClick="return confirm('هل أنت متأكد من حذف هذا الطالب؟');" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle CssClass="pager-style" />
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <!-- Search Tab -->
            <div id="search" class="tab-content">
                <div class="search-container">
                    <h2><i class="fas fa-search"></i> البحث المتقدم</h2>
                    
                    <div class="search-form">
                        <div class="form-group">
                            <label for="txtSearchName">البحث بالاسم</label>
                            <asp:TextBox ID="txtSearchName" runat="server" CssClass="form-control" 
                                placeholder="أدخل اسم الطالب"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <label for="txtSearchID">البحث برقم الطالب</label>
                            <asp:TextBox ID="txtSearchID" runat="server" CssClass="form-control" 
                                placeholder="أدخل رقم الطالب"></asp:TextBox>
                        </div>
                        
                        <div class="form-group">
                            <label for="ddlSearchDepartment">البحث بالقسم</label>
                            <asp:DropDownList ID="ddlSearchDepartment" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">جميع الأقسام</asp:ListItem>
                                <asp:ListItem Value="علوم الحاسوب">علوم الحاسوب</asp:ListItem>
                                <asp:ListItem Value="هندسة البرمجيات">هندسة البرمجيات</asp:ListItem>
                                <asp:ListItem Value="تقنية المعلومات">تقنية المعلومات</asp:ListItem>
                                <asp:ListItem Value="الأعمال">الأعمال</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-group">
                            <label for="ddlSearchStatus">البحث بالحالة</label>
                            <asp:DropDownList ID="ddlSearchStatus" runat="server" CssClass="form-control">
                                <asp:ListItem Value="">جميع الحالات</asp:ListItem>
                                <asp:ListItem Value="نشط">نشط</asp:ListItem>
                                <asp:ListItem Value="خريج">خريج</asp:ListItem>
                                <asp:ListItem Value="منقطع">منقطع</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        
                        <div class="form-actions">
                            <asp:Button ID="btnSearch" runat="server" Text="بحث" 
                                CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                            <asp:Button ID="btnClearSearch" runat="server" Text="مسح" 
                                CssClass="btn btn-secondary" OnClick="btnClearSearch_Click" />
                        </div>
                    </div>
                    
                    <div class="search-results">
                        <asp:GridView ID="gvSearchResults" runat="server" CssClass="students-table" 
                            AutoGenerateColumns="False" Visible="False">
                            <Columns>
                                <asp:BoundField DataField="StudentID" HeaderText="رقم الطالب" />
                                <asp:BoundField DataField="StudentName" HeaderText="الاسم الكامل" />
                                <asp:BoundField DataField="Email" HeaderText="البريد الإلكتروني" />
                                <asp:BoundField DataField="Department" HeaderText="القسم" />
                                <asp:BoundField DataField="Status" HeaderText="الحالة" />
                                <asp:BoundField DataField="GPA" HeaderText="المعدل" DataFormatString="{0:F2}" />
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="Scripts/main.js"></script>
</body>
</html>
