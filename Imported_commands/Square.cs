
string name ="";
if (code[1] == "ReFs")
{
     name = D.referenceS(code[2]);
        }
else if (code[1] == "ReFd") 
{
     name = D.referenceS(code[2]);
        } else 
{
     name = code[1];
        }

D.setD("xpos" + name, 1);
D.setD("ypos" + name, 0);
D.setD("zpos" + name, 0);