<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CombinedServicesTryItPage._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>CombinedServicesTryIt Page</h1>
        <p class="lead">This is the Master TryIt page for all services created by Bradley McGarvin. The tryit page is meant to allow for testing of each service in succession. It will first start by asking our user to input a valid zipcode (error handling enabled). This zip code will&nbsp; be converted to specific latitude and longitude coordinates. These coordinates will then be used in order to invoke our NaturalHazards (Earthquake Index) service. If a user trys to invoke the service before converting a zip code, they will receive an error message. At the bottom of the page will be our NewsFocus Search bar, allowing our user to search any topic they&#39;d like. In return, they will recieve the top 10 urls related to their search. Lastly, after invoking this service, the user will be given the option to analyze any URL they select using our Top10Words service. This service will look at the HTML script of the webpage and retrieve the top 10 words from the inner text of the body, essentially 
            eleminating tags along with whitespace.</p>
        <p class="lead">Click to go to index: <a href="http://webstrar61.fulton.asu.edu/index.html">http://webstrar61.fulton.asu.edu/index.html</a> </p>
     </div>
         <div style="margin-left: auto; margin-right: auto; text-align: center;">

        
             Insert Zip Code:<asp:TextBox ID="TextBox1" runat="server" Width="206px"></asp:TextBox>
             <asp:Label ID="ZipError" runat="server" Font-Italic="True" Font-Size="X-Small" ForeColor="#CC3300"></asp:Label>
             <br />
             <br />

        
    </div>

            <div style="margin-left: auto; margin-right: auto; text-align: center">


                





                Date Range:<asp:DropDownList ID="DropDownList1" runat="server" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    <asp:ListItem Value="10">10 years</asp:ListItem>
                    <asp:ListItem Value="25">25 years</asp:ListItem>
                    <asp:ListItem Value="50">50 years</asp:ListItem>
                    <asp:ListItem Value="100">100 years</asp:ListItem>
                </asp:DropDownList>
&nbsp; Radius:<asp:DropDownList ID="DropDownList_Radius" runat="server" OnSelectedIndexChanged="DropDownList_Radius_SelectedIndexChanged">
                    <asp:ListItem Value="10">10 miles</asp:ListItem>
                    <asp:ListItem Value="25">25 miles</asp:ListItem>
                    <asp:ListItem Value="50">50 miles</asp:ListItem>
                    <asp:ListItem Value="100">100 miles</asp:ListItem>
                </asp:DropDownList>
&nbsp; Magnitude:<asp:DropDownList ID="DropDownList_Magnitude" runat="server" OnSelectedIndexChanged="DropDownList_Magnitude_SelectedIndexChanged">
                    <asp:ListItem>2.5</asp:ListItem>
                    <asp:ListItem>3.0</asp:ListItem>
                    <asp:ListItem>3.5</asp:ListItem>
                    <asp:ListItem>4.0</asp:ListItem>
                    <asp:ListItem>4.5</asp:ListItem>
                    <asp:ListItem>5.0</asp:ListItem>
                    <asp:ListItem>5.5</asp:ListItem>
                    <asp:ListItem>6.0</asp:ListItem>
                </asp:DropDownList>
                <br />
                <br />


                





            </div>
        
        
            <div style="margin-left: auto; margin-right: auto; text-align: center">

                

                <asp:Button ID="Convert_Zip_Btn" runat="server" OnClick="Convert_Zip_Btn_Click" Text="Convert Zip" Width="170px" />
                <br />
                <br />
                Latitude:<asp:Label ID="Lat_Output" runat="server"></asp:Label>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Longitude:<asp:Label ID="Long_Output" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Button ID="Earthquake_Index_Btn" runat="server" OnClick="Earthquake_Index_Btn_Click" Text="Earthquake Index" Width="168px" />

                

            </div>
        
        
            <div style="margin-left: auto; margin-right: auto; text-align: center">

                

                <asp:Label ID="Label1" runat="server" Text="The Earth Quake Index is:"></asp:Label>
                <asp:Label ID="Index_Label" runat="server"></asp:Label>
                <br />

                

           </div>

    <div style="margin-left: auto; margin-right: auto; text-align: center">

        

        <asp:Label ID="ErrorLabel" runat="server" Font-Bold="True" Font-Overline="True" Font-Size="Medium" Font-Underline="True" ForeColor="#CC3300"></asp:Label>

        

        <br />

        

        </div>

    <div style="margin-left: auto; margin-right: auto; text-align: center;">

        <asp:Label ID="NewsFocus_Label" runat="server" Text="NewsFocus Search Bar"></asp:Label>
        <asp:TextBox ID="NewsFocus_TextBox" runat="server" Width="384px"></asp:TextBox>
        <asp:Button ID="Search_Btn" runat="server" OnClick="Search_Btn_Click" Text="Search" Width="118px" />

        <br />
        <br />

        </div>

    <div style="margin-left: auto; margin-right: auto; text-align: center;">
            <asp:Label ID="TopTenLabel" runat="server" Font-Bold="True" Font-Size="Medium" Text="Select a URL and click Top10 to retrieve the Top 10 Content Words from that URL" Visible="False"></asp:Label>
            <br />
    </div>

        <div style="margin-left: auto; margin-right: auto; text-align: center;">

            

            <asp:ListBox ID="URL_Listbox" runat="server" Rows="12" Visible="False" Width="865px" OnSelectedIndexChanged="URL_Listbox_SelectedIndexChanged"></asp:ListBox>

        </div>

        <div style="margin-left: auto; margin-right: auto; text-align: center;">

            <br />
            <asp:Button ID="Top10_Btn" runat="server" Text="Top10" Visible="False" Width="136px" OnClick="Top10_Btn_Click" />

            <br />

        </div>

    <div style="margin-left: auto; margin-right: auto; text-align: center;">

        <asp:Label ID="Top10WordsFor_Label" runat="server" Text="Top10Words for " Visible="False"></asp:Label>
        <asp:Label ID="Top10_URL_Label" runat="server" Visible="False"></asp:Label>
&nbsp;<asp:Label ID="Colon_Label" runat="server" Text=":" Visible="False"></asp:Label>
        <asp:Label ID="TopTenOutputLabel" runat="server" Visible="False"></asp:Label>

        </div>

</asp:Content>
