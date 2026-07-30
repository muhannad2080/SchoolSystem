import os

main_path = 'MainForm.cs'
with open(main_path, 'r', encoding='utf-8') as f:
    main_content = f.read()

main_content = main_content.replace('LoadUserControl(new SchoolSystem.UI.Students.EnrollmentForm());', 'LoadFormInPanel(new SchoolSystem.UI.EnrollmentForm());')

with open(main_path, 'w', encoding='utf-8') as f:
    f.write(main_content)

if os.path.exists(r'UI\Students\EnrollmentForm.cs'):
    os.remove(r'UI\Students\EnrollmentForm.cs')
if os.path.exists(r'UI\Students\EnrollmentForm.Designer.cs'):
    os.remove(r'UI\Students\EnrollmentForm.Designer.cs')
if os.path.exists(r'UI\Students\EnrollmentForm.resx'):
    os.remove(r'UI\Students\EnrollmentForm.resx')

print("Fixed MainForm successfully!")
