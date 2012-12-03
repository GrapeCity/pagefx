using System;

public class CredentialSoapHeader
{
    //WARNING: We can use System.String because flex does not correctly serialize it to xml
    public Avm.String Username;
    public Avm.String Password;

    public CredentialSoapHeader()
    {
    }

    public CredentialSoapHeader(string user, string password)
    {
        Username = user;
        Password = password;
    }
}