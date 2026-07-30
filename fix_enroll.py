import os
import shutil

cs_path = r'UI\EnrollmentForm.cs'
with open(cs_path, 'r', encoding='utf-8') as f:
    cs_content = f.read()

cs_content = cs_content.replace('class EnrollmentForm : Form', 'class EnrollmentForm : UserControl')
cs_content = cs_content.replace('this.Close();', '')
cs_content = cs_content.replace('namespace SchoolSystem.UI', 'namespace SchoolSystem.UI.Students')

os.makedirs(r'UI\Students', exist_ok=True)

with open(r'UI\Students\EnrollmentForm.cs', 'w', encoding='utf-8') as f:
    f.write(cs_content)

des_path = r'UI\EnrollmentForm.Designer.cs'
with open(des_path, 'r', encoding='utf-8') as f:
    des_content = f.read()

des_content = des_content.replace('namespace SchoolSystem.UI', 'namespace SchoolSystem.UI.Students')
des_content = des_content.replace('this.ClientSize =', 'this.Size =')

lines = des_content.split('\n')
new_lines = []
for line in lines:
    if 'this.Text =' in line or 'this.RightToLeftLayout =' in line:
        continue
    new_lines.append(line)

with open(r'UI\Students\EnrollmentForm.Designer.cs', 'w', encoding='utf-8') as f:
    f.write('\n'.join(new_lines))

if os.path.exists(r'UI\EnrollmentForm.cs'):
    os.remove(r'UI\EnrollmentForm.cs')
if os.path.exists(r'UI\EnrollmentForm.Designer.cs'):
    os.remove(r'UI\EnrollmentForm.Designer.cs')

print("Fixed successfully!")
