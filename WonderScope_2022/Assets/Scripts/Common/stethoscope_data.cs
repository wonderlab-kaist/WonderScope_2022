using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stethoscope_data
{
    public float[] q;
    public float[] a;
    public byte[] tag_id;
    public float distance;
    public float mouse_x;
    public float mouse_y;
    public float battery;
    public long time_stamp;

    public stethoscope_data()
    {
        
        q = new float[3];
        a = new float[3];
        tag_id = new byte[4];
    }

    public stethoscope_data(byte[] data_in)
    {
        if (data_in == null) return;
        q = new float[3];
        tag_id = new byte[4];
        union_float uf;
        union_int ui;
        
        uf.f = 0;
        uf.b0 = data_in[0];
        uf.b1 = data_in[1];
        uf.b2 = data_in[2];
        uf.b3 = data_in[3];
        q[0] = uf.f;
        
        uf.b0 = data_in[4];
        uf.b1 = data_in[5];
        uf.b2 = data_in[6];
        uf.b3 = data_in[7];
        q[1] = uf.f;

        uf.b0 = data_in[8];
        uf.b1 = data_in[9];
        uf.b2 = data_in[10];
        uf.b3 = data_in[11];
        q[2] = uf.f;

        tag_id[0] = data_in[12];
        tag_id[1] = data_in[13];
        tag_id[2] = data_in[14];
        tag_id[3] = data_in[15];

        uf.f = 0f;
        uf.b0 = data_in[16];
        uf.b1 = data_in[17];
        uf.b2 = data_in[18];
        uf.b3 = data_in[19];
        distance = uf.f;
        
        uf.b0 = data_in[20];
        uf.b1 = data_in[21];
        uf.b2 = data_in[22];
        uf.b3 = data_in[23];
        mouse_x = uf.f;
        
        uf.b0 = data_in[24];
        uf.b1 = data_in[25];
        uf.b2 = data_in[26];
        uf.b3 = data_in[27];
        mouse_y = uf.f;
        
        uf.b0 = data_in[28];
        uf.b1 = data_in[29];
        uf.b2 = data_in[30];
        uf.b3 = data_in[31];
        battery = uf.f;


    }

    public stethoscope_data(byte[] data_in, int ver_)
    {
        
        if (data_in == null) return;
        q = new float[3];
        a = new float[3];
        tag_id = new byte[4];
        union_float uf;
        union_int ui;
        union_16int u16i;

        if (ver_ == 2) {
            uf.f = 0;
            uf.b0 = data_in[0];
            uf.b1 = data_in[1];
            uf.b2 = data_in[2];
            uf.b3 = data_in[3];
            q[0] = uf.f;

            uf.b0 = data_in[4];
            uf.b1 = data_in[5];
            uf.b2 = data_in[6];
            uf.b3 = data_in[7];
            q[1] = uf.f;

            uf.b0 = data_in[8];
            uf.b1 = data_in[9];
            uf.b2 = data_in[10];
            uf.b3 = data_in[11];
            q[2] = uf.f;

            uf.b0 = data_in[12];
            uf.b1 = data_in[13];
            uf.b2 = data_in[14];
            uf.b3 = data_in[15];
            a[0] = uf.f;

            uf.b0 = data_in[16];
            uf.b1 = data_in[17];
            uf.b2 = data_in[18];
            uf.b3 = data_in[19];
            a[1] = uf.f;

            uf.b0 = data_in[20];
            uf.b1 = data_in[21];
            uf.b2 = data_in[22];
            uf.b3 = data_in[23];
            a[2] = uf.f;


            u16i.i = 0;
            u16i.b0 = data_in[24];
            u16i.b1 = data_in[25];
            distance = u16i.i;

            u16i.i = 0;
            u16i.b0 = data_in[26];
            u16i.b1 = data_in[27];
            mouse_x = u16i.i/100;

            u16i.b0 = data_in[28];
            u16i.b1 = data_in[29];
            mouse_y = u16i.i/100;

            tag_id[0] = data_in[30];
            tag_id[1] = data_in[31];
            tag_id[2] = data_in[32];
            tag_id[3] = data_in[33];

            battery = data_in[34];

        }
        else if (ver_ == 1)
        {
            ui.i = 0;
            ui.b0 = data_in[0];
            ui.b1 = data_in[1];
            ui.b2 = data_in[2];
            ui.b3 = data_in[3];
            q[0] = ui.i;

            ui.b0 = data_in[4];
            ui.b1 = data_in[5];
            ui.b2 = data_in[6];
            ui.b3 = data_in[7];
            q[1] = ui.i;

            ui.b0 = data_in[8];
            ui.b1 = data_in[9];
            ui.b2 = data_in[10];
            ui.b3 = data_in[11];
            q[2] = ui.i;

            tag_id[0] = data_in[12];
            tag_id[1] = data_in[13];
            tag_id[2] = data_in[14];
            tag_id[3] = data_in[15];

            uf.f = 0f;
            uf.b0 = data_in[16];
            uf.b1 = data_in[17];
            uf.b2 = data_in[18];
            uf.b3 = data_in[19];
            distance = uf.f;

            uf.b0 = data_in[20];
            uf.b1 = data_in[21];
            uf.b2 = data_in[22];
            uf.b3 = data_in[23];
            mouse_x = uf.f;

            uf.b0 = data_in[24];
            uf.b1 = data_in[25];
            uf.b2 = data_in[26];
            uf.b3 = data_in[27];
            mouse_y = uf.f;

            uf.b0 = data_in[28];
            uf.b1 = data_in[29];
            uf.b2 = data_in[30];
            uf.b3 = data_in[31];
            battery = uf.f;
        }



    }
}
