using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.MCDM_Functions
{
    public class CriteriaScoreCalculation
    {
        private static double Wfp_rVAlue(double CNt, double CNr)
        {
            double Wfpr = default;
            if ((CNt / CNr) < 5) Wfpr = 0;
            if ((CNt / CNr) > 20) Wfpr = 1;
            else Wfpr = 0.5;

            return Wfpr;
        }
        
        public static double FabricationAndProcessingScore(double CNt, double CN1, double CN2, double CN3, double CN4, double FABeff, double CNr, double Ppc, 
            double Mp, double M, double Wfp1, double Wfp2, double Wfp3, double Wfp4, double Wfp_pc, double Wfp_r)
        {
            double complexPart = (Wfp1 * CN1) + (Wfp2 * CN2) + (Wfp3 * CN3) + (Wfp4 * CN4);
            double complexPartB = (Wfp_pc * Ppc) + Mp - (CNt * Wfp_r);
            double FabandProcessingScore = ((1 - FABeff) * (1 / M) * complexPart) + ((1 / M) * complexPartB);

            return FabandProcessingScore;
        }

        public static double ProcurementScore(double Pstd_c, double Pstd_o, double Pcus, double Pna_aus, double Pp_std, double Ppna_aus,
            double Ppm, double Wmstd_c, double Wmstd_o, double Wmcus,
            double Wmna_aus, double Wpstd, double Wpna_aus)
        {
            double complexPart = (Wmstd_c * Pstd_c) + (Wmstd_o * Pstd_o) + (Wmcus * Pcus) + (Wmna_aus * Pna_aus);
            complexPart = (1 - Ppm) * complexPart;
            double ProScore = Ppm * ((Wpstd * Pp_std) + (Wpna_aus * Ppna_aus));

            return complexPart + ProScore;
        }

        public static double MassPerSquareMeter(double m, double A) { return 1000 * m / A; }

        public static double TreatmentScore(double Asc, double Atc, double Ahdg, double Afire, double Aut, double M, double Wptsc, double Wpttc,
            double Wpthdg, double Wptfire, double Wptut)
        {
            double complexPart = (Wptsc * Asc) + (Wpttc * Atc) + (Wpthdg * Ahdg) + (Wptfire * Afire) + (Wptut * Aut);
            
            return (1/M) * complexPart;
        }

        public static double SiteConstructionTimeScore(double M, double Tsite_allow, double Tdelay, double Tcout, double Tlift, double Nlifts, 
            double Ncranes, double Npwf, double CNa, double Cnb, double Cnc, double Wcna, double Wcnb, double Wcnc)
        {
            double WTF = Tlift / 60;
            WTF = (Nlifts / Ncranes) * WTF;
            double complexPart = WTF + ((1 / Npwf) * ((Wcna * CNa) + (Wcnb * Cnb) + (Wcnc * Cnc)));
            double front = (1 + Tdelay + Tcout);
            double frontB = M * Tsite_allow;
            double SCT = (front / frontB) * complexPart;
            
            return SCT;
        }

        public static double TotalOnsiteLabourTime(double M, double Nppl, double Tdelay, double Tcout, double Tlift, double Nlifts,
            double CNa, double Cnb, double Cnc, double Wcna, double Wcnb, double Wcnc)
        {
            double complexPart = (Tlift * Nlifts / 60) + ((Wcna * CNa) + (Wcnb * Cnb) + (Wcnc * Cnc));

            return (Nppl / M) * (1 + Tdelay + Tcout) * complexPart;
        }

        public static double TransportationScore(double Mtruck, double Ttruck, double Pstd_tr, double Pesc_tr, double Ppol_tr, double Dtruck, double M,
            double Wpstd_tr, double Wpesc_tr, double Wppol_tr, double Wttruck)
        {
            double complexPart = Dtruck * ((Wpstd_tr * Pstd_tr) + (Wpesc_tr * Pesc_tr) + (Wppol_tr * Ppol_tr));
            double finalComplex = (Wttruck * Ttruck) + complexPart;

            return (1/M) * Mtruck * finalComplex;
        }

        public static double SustainabilityScore(double TRANS, double w_dc, double w_rec, double w_reu, double Pwaste, double Pgs, double Pdfd, double M, 
            double Pstdc, double Pstdo, double Pcus, double Pna_aus, double Ppstd, double Ppm,
            double Wco2_stdc = 1700, double Wco2_stdo = 1600, double Wco2_cus = 1800, double Wco2_na_aus = 1800, 
            double Wco2_pna_aus = 1600, double Wco2_gs = 1100, double Wco2_tra = 0.8, double Wdfd = 1100, double W1dfd = 20, 
            double Wrec = 1500, double Wreu = 2500)
        {
            double complexPart = (Wco2_stdc * Pstdc) + (Wco2_stdo * Pstdo) + (Wco2_cus * Pcus) + (Wco2_na_aus * Pna_aus) + (Wco2_gs * Pgs);
            complexPart = complexPart * (1 - Ppm);
            complexPart = complexPart + (Wco2_stdc * Ppm * Ppstd) - (Wdfd * W1dfd * Pdfd);
            complexPart = complexPart * (1 + Pwaste);

            double Front = (TRANS * Wco2_tra) + (1 / M) * ((Wreu * w_dc - w_reu) + (Wrec* w_rec));
            return Front + complexPart;
        }

        public static double SimplifiedCost(double Ncranes, double FPS, double Pna_aus, double SCT, double Rdes_det = 7,  double Rbld_ohpr = 15, double Rdes_cont = 5, double Resc = 6, double Rfab_ohpr = 30,  double Rfps = 55,
             double Rinstall = 1200, double Rcrane = 12000, double Rpr = 4000,  double Rpr_naNaus = 3500)
        {
            double complexPart = ((1 + Rfab_ohpr + Rdes_det) * (Rfps * FPS)) + (Rinstall + (Rcrane * Ncranes *SCT)) + (Rpr * (1 - Pna_aus) + (Rpr_naNaus * Pna_aus));

            return (1 + Rbld_ohpr + Rdes_cont + Resc) * complexPart;
        }

        public static double DetailedCost( double FPS, double TS, double Ncranes, double SCT, double Trans, double m, double TOSLT, double Pcus,double Ppna_aus, double Pna_aus,
            double Pstdc, double Pp_std, double Ppm, double Pstdo, double Cspec = 5000, double Rdes_cont = 7, double Rfps = 55,  double Rtoslt = 80,  double Rcrane = 4.5, 
            double Rtrans = 4.5, double Rfab_ohpr = 30, double Rdes_det = 7,  double Rm_stdc = 2200, double Rm_stdo = 1900,  double Rbld_ohpr = 15, double Resc = 6,
            double Rm_cus = 2400,  double Rmna_aus = 1700,  double Rp_std = 1700, double Rpna_aus = 1600)
        {
            double curlyBracket = ((1 - Ppm) * ((Rm_stdc * Pstdc) + (Rm_stdo * Pstdo) + (Rm_cus * Pcus) + (Rmna_aus * Pna_aus))) + (Ppm * ((Rp_std * Pp_std) + (Rpna_aus * Ppna_aus)));
            double secondLine = (1 + Rbld_ohpr + Rdes_cont + Resc) * (1 + Rfab_ohpr + Rdes_det) * curlyBracket;

            double complexPart = (Rfps * FPS) + TS + (Rtoslt * TOSLT) + (Rcrane * Ncranes * SCT) + (Rtrans * Trans) + (Cspec / m);

            return (1 + Rbld_ohpr + Rdes_cont) * complexPart + secondLine;
        }
    }
}
