using EduK.IS.Data;
using EduK.IS.Models;
using IdentityModel;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EduK.IS
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();

                {
                    var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                    context.Database.Migrate();
                    EnsureSeedData(context);
                }

                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var lilo = userMgr.FindByNameAsync("175.504.618-99").Result;
                    if (lilo == null)
                    {
                        lilo = new ApplicationUser
                        {
                            UserName = "175.504.618-99"
                        };
                        var result = userMgr.CreateAsync(lilo, "Lilo$123").Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }

                        result = userMgr.AddClaimsAsync(lilo, new Claim[]{
                            new Claim(JwtClaimTypes.Name, "Liluyoud Cury de Lacerda"),
                            new Claim(JwtClaimTypes.GivenName, "Professor"),
                            new Claim(JwtClaimTypes.FamilyName, "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD/4QBmRXhpZgAATU0AKgAAAAgABgESAAMAAAABAAEAAAMBAAUAAAABAAAAVgMDAAEAAAABAAAAAFEQAAEAAAABAQAAAFERAAQAAAABAAAOw1ESAAQAAAABAAAOwwAAAAAAAYagAACxj//bAEMAAgEBAgEBAgICAgICAgIDBQMDAwMDBgQEAwUHBgcHBwYHBwgJCwkICAoIBwcKDQoKCwwMDAwHCQ4PDQwOCwwMDP/bAEMBAgICAwMDBgMDBgwIBwgMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDAwMDP/AABEIAIQAfAMBIgACEQEDEQH/xAAfAAABBQEBAQEBAQAAAAAAAAAAAQIDBAUGBwgJCgv/xAC1EAACAQMDAgQDBQUEBAAAAX0BAgMABBEFEiExQQYTUWEHInEUMoGRoQgjQrHBFVLR8CQzYnKCCQoWFxgZGiUmJygpKjQ1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoOEhYaHiImKkpOUlZaXmJmaoqOkpaanqKmqsrO0tba3uLm6wsPExcbHyMnK0tPU1dbX2Nna4eLj5OXm5+jp6vHy8/T19vf4+fr/xAAfAQADAQEBAQEBAQEBAAAAAAAAAQIDBAUGBwgJCgv/xAC1EQACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8/T19vf4+fr/2gAMAwEAAhEDEQA/APdvE/jvULDxbqzf2vqGxbmQbRMxxhjWXb/GK6nl/c6xqXmf3fNf/Gvn34p/tczaV4u8QWs3hy6t72HUJ1Dxyi4hQCRhlio7j8uK5lv2kdUMsF1bSPewu4TZ8saqT1yvUD/awTWUqi2MIQly80j6dufidqd7DMi6pqihuN3nOOcH37VxXwX+KfiBr67sbjxFfTzWuCkUkjLJtJLKxOeeK8u1f9pHULTw95SaXIdQBH7yO4DxxZByQwAPBxxjJrl/gf8AEfUtL1vWNXvpI/MvNq4lGWjQEkbQecHOMk/hWErct5F04Pn93c+rfiJ8f7rwl4OuLi71jUI2kXZGiSOzFiPr+OfQE9BXF+Ff2hD4St3ku9Y1aO6u/wB60RuHfaOOc5wB9evbNfKv7V3xxvZNXs7/AEvV5mt4ZJAtrKvyox25XPRlwMYrwW6/aFvL+5VrnV7qMbPKILHbjP3cemOPoK6qMuaNnsdFTC6p9T9AfGP7c0d9qUdtY6trM1/bSkSD7S0SkYzgc/MRyeOa5z4zfHvVvil8MdPvtP1LXNP8RaPdCezh+0uguVbCypw/zKy7T82MFB718Rr43t5IJpku/scq7THMJAA2TgYPvyfbvXO3vxa1z+0d11Nc3JVittOkjFSQRncM8HGD7jNdVPD6qUehz1qcZRcGfYHg/wCKnjHUdUZbzxB4g8sshjia6m4KsCw3ZPHfmvrD4WfHjUNZC2surXCurbfLN0xc+/U1+XPgr9oaZdStLdbhmdG8qRWP+uYnOSzdc8D2xX078DPippvi3VbSSS6m0+8tZdzuX2OQM8dwR159K5cywdWaTgLAxVFtH6JeHvG18f8Al/um/wC2pP8AWursfEN5JDu+3XO4jp5mc15F4H8S2eqaLbz2dxb3UTKB5kUokUnjPzDrjNdlDriW1s2X24Tg9c1yxw84R943+sc8rI6yz8SXjnP2y4H/AAM/41al8U3hOft1xxz9/wD+vXIJqTIV2/lQ+t4Qhh+RrjlW5dz0qeHc1zHXS+MbwRgreXHynpvP+NeYaz+3/wCHfAOs3mh6hda4t5pM720pWIOrFSeQfQjFbJ18+QrblUSNtGf4e+DXyT8dP2cPF/jz4qazrXh2zsZtM1K5kmR1uURmYsQSwcjDcD7uRjHJOa+g4Vo5HjMVOGcz5YqOmttb6/gebxBTzSjh4yy2HM766X/M8B+O3je4uvil4gktF+zWY1S5H2fGRIPMIJPqe/5VwMWqo6P5fmRtk/c4ODXpXxj0SNPH3iJZLQ25i1O5/ecMr5lbv7153P4fkhl3rHt6/N3JrzJYeUVoxqaehqeHdXu57do4Ziyn5pEZuW7Vyfjz4lW/gXUrhbOO6a6uF++0m6IHvjnt9KseMvFjeBPBd5ezSQxtHGfKCDLO3bI/M/hXzHf+PL7xdq7NJJvZQfnX05/xp4eN9Zao6VRjF36m58RfireeLNRX7VcSsVJ2IThR9AOPxrz+719pb1mZWb5umc4q1eS/bdZPlfMxIwB1Pauq8Bfs3eM/ibqq2+l6W0nmgjJBHHrnGK9CWKpQjaeiJjhq1SVqepyL66jSLtUqr9s5XNbEGv3EE0IjPmOpwCRzjqBn8xz2OK9QvP8AgnR8RFuVWDQriZsDLF1AU/XPSvXPgj/wSr8UayGk15o7FewRt/5EVzrNsNHaaNamU4paygfLYszAfPt/MVpCSUbnb7fStzwv8RrzSrqF7O5l03UbUg7TIfIusdM8/K3HDj/gQYcV9lat/wAEn5NuE1S4XI4MZ+Ye3IryL4rf8E5PGng+OS4014dYgjXOx08uc89AehNdFHNsPPRswqZZXguZI9T/AGVP+CmS/D149J8WWc2oWsjKsbwkJN90sSFJx5nUFCdrlflZMhW++Phf8VdC+NPgu31nw7fQalptwB80bbXjYjPlyJ1jkHdWwfTI5P4+/s1/B6y+JnxovPCHim+k0GSGwlmWO6HkzPIrICq7iAWVSzDnDBRjPOPXv2Nvizd/szftI6hZ32t2Wn6bb2moaZfzbzNayNG5e3mKAEtmQI2AA2xiAQcZ2lUpSbsefUwk3qtz9WbC2uWixl/xOcUk2iXzBVVlZeuM7efqa7fwppmgvZwytfG9aZQStrymehCkdcNx+FdrpHhnT51yumNtx8ry5bPtyf5ZFeZiqeGm7Pc6sHUxNFWufL3ibwb4li1FfJs5JLUvkGOQMRn8etY3i3R/G2m2+jxaHDbLafYAzmeNmdpTLIXzgdj8v/Aa+xGWxsvlT7CrDLERoHb3zjIA9zQdctUOFVcfnj8ga+dxGBwzlrM+mwmeYilDl5EfmL4h+DNjqPi/VLhmvl82+nDxyOCu7zWJBUdevXIqzH+zBos/zSWrHAzj7QQB+C16p418LS674n1K0ignh3XcpEkY3MoMhOea2bTwTNpkECL5n2RjseR+W3Htn6dq9etma5tHZHy+EwitzS1Z+a//AAUN0rTfC963hrTE8tbNEa6wf+WjjdjPXhcfnXzXY6VFp1leTW4cLHEcSOAMsRjAFfQ37ZELah4m8YXk1vJDctrs6eWyncuHKd/QKPyrxSaBv+EYZQn3V3Yx/KtsNWi6d13PcxGFkpfIZ+zP8N/+FlfESytprhIELL98bi47gV+yH7N/7O+keEfCNi8VvG7eWrKxHzZ7fpX5X/sS6PHafFbS7idljhiPmSOR0xjj8a/Zb4RzrL4V09guweWCFI9q+f4iqL2qj0PcyWnKOHbW7Ny28FwxjCwrt9O1Wx4VjjH+qVc+g61fjulSL17VHcaiqj73SvLo8id7He6c2jJvvCsDdEwfpWHq/giGdz+7XPqBzXWf2lkE5Xj1qB7iNgN2Du4r1IzRwVKLPFPiZ+yh4M+K4t18ReHNL1cWr+ZF9pgVih9j1Ge4B5Fcz8Vv+Cavw8+K3gyK00PRdK8E6zawmGwv9JtltkjOG2rOijEke5gTn5gOQwPX6Ju7aMDdt60/SbYXETIO3zD8K6o4iad07HnyopLmkjI/ZS8Sah4i+BWjLZ2OkeFx4egOiajBPcbptPvLP93PHJt4B3AOCAAVkU87hXZ2Q+zwxpD4gvtVdxuZ5GIjB/2c9R6E9q4n4S/D64g/aG8cW1paRxaT4k06x8QzT7ysZ1BXls5sqPvO0cNsxI7Y56GvYI/B1hob7Lia6v5QNxQYjjB6Yz1P41g6cZS5orUxdoSsjDsG1AMqxzSXDKdyIw8xR7gdAffFWriW6lkzcahdLL0IWYNj+ePpXQW+m3WuR+Va2rQ2/TanyRn6nvV6H4cT+WN1xDGfRYyR/Sqjl8qmvKOWMhDSTMFv2f8ATtJ8RXUxmklkNy8hZUxkEk7T9Kt3Hw00mGzkja1WWGRt7K/KnqM4+hNega0gXVbj5fvOfxrOvrPfEflZRjHPSvno1uatprY86UnFe7oj8f8A/grF8FovAn7RF9NZ2aQ6X4igi1KKNQQok2qspB/39zf8Cr4rvdKW2gkhiVm8uQLx/dPav1s/4LHfDO/8X/D3wxdaXpuoalfabczs62kJkeODALswHO3Nfmjo3g//AImMjSKZFJ8yUgYwccAV72BxXuSXZn00aMq1KnU8jb/ZR8DLp3iITS2/mPuXyEB++5IKj0wO+fSv1B+CeuNd+HYlebz2hGJJAMKW6ED6Hj8K+F/2WfhY3jjxpa2e5oVjxJIV+8gHLAe54GfSvv7wV4Zi8NaBDaW8KwxxqQqKOF5rw84xCqTPewuGjSpqB0M+pnZ/EPwrNl1HzWxu/wDr0shYv8u0L71z/iLx/o/g3LXkzbVG52ClljHU5I6Vz4KfMrM6qyjCOiuzpInklX5d1TCzmyDyRmvJbb9r2x1C7KaTpNxqMAYjzo22qecd8V6B4K+KbeJoGabTLywdTgLKytu9wRXuUpQitWeHVnVf2TpDHJLb7WBVv0qKCb+zn8zdyp54p15r8aWTv932Jr5v/al/ahtfBNnJaw67Z6TcNwZX+byVxy7AZOMZxXRGSk7R1OGUZWdz6o8C67BP4n2294trNeW5hODknBDYwOex6V6r4K0bTLwebH5l5JGcFpV2qD7D+p5r8xP2fviFHb/FeyfT/HGralrNvMk00N1ZrAUI+YqFY+YoKgjkAHNfqXaeIrTT0VGZVkcAiGMZbd6YHp716GBjDnafQ8XMI1IxjJfaOgggUJhR8o6D0pzyLGfmaNT6M2P51kk6lqaA7l0+FhkjG6Yj+QqrcaZpdjLtmi+0SN8xeV8sa9KpiOVXijyvZpvV39Dd1RY01G4Xp85PJzVaSPchB5rQ1tRDqc7YP+sxlhVdrlQufl9sDjNfI4PDwjUaqySa7dTRyfQ8Z+PvhWS98RaKWZ/sN2GtplH3X4yAfrtNfnL+01+wvrPwq8R6z4j0NftXheaQXEsaLulsVZgCCOrLyDkdB1xX6t+PdM/tbRZN6rugPmxkjow//Wa8Eure71TxNMrXTCOOMr9lf/VzBvlO5T94EcYrzMXJ0MVOVBPldtz9ByHFe0wihK3ubnyb+wLo0dj4j1Yuq+ZJh4yPmCockfmMflX11b2v7pcdOn0r520XwknwP/aK1CwsY1j0m5nTyU/54xOu+NB7IWZB7AV9L2Fossadcn9K8zEXlPm7nsYiKg1NbM43xhczQq0aMF45Pp+fT6183/GD43/2Bqc2n6HomreLNXjQy/ZrS2MyKAcFnfBjXnoCWdsZwBgn651/wcuoxMNsZ3D0rktR8HajosD/AGcW+1hjCRhP5CurCUZJptXRKxNP2bhezPkHwp4t+KHxA0bUry6hvPCs1lcRw2Wjf2U/nXiFSXcyfKsaLjq4GS33TjJ9s/Z6bXddsT/bV3MJEfbsRgcAepXjP0Arqrrwffa9O0N5LcLDIcSKrYDD0OOo9jXoPhHwhp+gWkcNvDDboOQMcn6162OcalO8Y8pw4WUsP7spczZNceGBqXhpoWVt23AIPOCK+NPHH7JXh3w3481CTWNG1jXJLty32w3jqWjY7tuFwNvbB9BX3pDEgjwrLx6Vynj3w/DeWrXCw+dPbAsqqMmUdSv5cj3FbZZKUKCcXqedinH2vLJXTPCPh38OND8SeMk1eTSnutTDL5mpakxurogdAJHycD0FfcXwgaG88NW90sSiZgYpZCcs7rwx9h0P418+eF72x1CCOW1eNo25BX/P6V6/8H/E4svD17Du3Ms/mIPTcoz/AOg16mFrRhVV3e6PIzShKdH3VazPRta8QJpsDbW3SsemK5Oe5e5lZ2Zizc9abcXT3Ts0jbmbvUInx610TqOesjzadNU9EerapZqNQmyu758/pUKWvl7igC/QVoasANQm/wB4/wAqrg5/OvQ+p0U7qK/r8Tx+ZmD4g0EX+n3I+Z5micRb2+UPjj9f514L4l8Ofa0h1SFWtdSVVcb87Mg/MGX65H8q+jbvkn/PevJPinZR6DqMzPuaO6BlQ/3Wycj6V8xxRheSEcRHRbP9D6vhfGSjOVDvqv1PnP4z6VE3ji4vZI4zLNbxmM5/1bIMgfTOOa9I+HHi2HxBo9vJ/wAtQgWQejgc/rXC/GaAtqKs23y5Y1O4HOD7foayfAOtv4a1rbvxFIcsPU47fXrXxHNzQ0Z+h1KftaC7o+goWWQdqbe2qPb9BtrnbXxZGsQbdtDDjNVdf8cxWlkztJtVc8k8V62X47lajPc+drYWb2M/xtqlp4cia4ZlVVznnv2H41zemeN4LS8S81G5jjt5lPAYfKfb8PSvjf8AbV/bIu/FPjj/AIRvw3OZLfTZR57Rji4nH8OePlXn2zznil8D+A/id460uSxv7qawWSMCNpYxJwQHHTcDjefmHHy+nNfR1MO50uebsc+FrSjUdOmubu72Pt2z/aI8G29x5a+INPO7AKvJtwfxrnfjt+0z4T+DfhabWtc1OK1s4xmCEOrXF85xtjgjzl2J4yOAOSQAa+TbT/gnZql/4gstYv8AxVawSW91HczQi08x3CkMVwCASeh5wf0rL8d/sFfDnwBry3VvpOsalq2qFjBHPMI4bY8lpJNqqVjJPEeSSQAOMmpw9GNN6zbj6F1qNSp8ELS7t3Oi/Zv/AG6NM8QePfscqS2K61PJJbwv9wOMs2OeMgHjNfdHwU8RR6xJqCx4b7OIGY+nmCQr+iE/iK/LH4e/DHQfgt+1PqV9dSzT6f4TtF8mJsECeWLLvgDkKu447EjqcCv0T/Ye1CbXfg23ii4jkhbxtqEur2kbAAxWO1YbTP8AvQxI/wBZPz6XhYrEKpT0jY8itjpTwrhW1mna59AeaAtRNPg8VUjn3KuWp+3/ADiuw8uMdNT3HV/+QnP/AL9VJJlhVmY4x61X8Ua/Fa6rcLDiaQOe/wAorAmS81g/8tNvbPAFd9TGR2p6s8unhW1zT0RZ1vxRHbo3lFZHx1J+Va8l+JOpyPrUc07MyNHsbP3R8x5/OvU18KINrXEm7H8C9K8u+PGvQ6bfxpHCslrbr5c6p99STnKjvtyOO/NfOcRRrTwl6mmqsj6Lh32axfJTV9HqeI/Gu0m0yPzP+WSnMZPp3X8O31rztvFHlTWMnmJ5jN5bKTyAM4z7EV7N4q0238WeH30+ScNb3ilrO6RtwU44GfbHHTOCOtfKvj1b7wf4qbTtYVre8jB2yDO2YAcOn4Y46iviqMW/dR+nYeSlDlluj2Kf4j3KWdxJnC2/OCeSAOn6iuK8efGl7/QrpNsuUVt4V/mY4ycf41yEXxgtZtKurc3EUlztJDKR8wPG3nvXmPjXXZLTdNDM3m27JEWQffUnJBHTt/OvWwWHUpJyVjhzBqEGomr+w/8ADbw3458dfEEXlrZ6xax6i8KrcQJKogkXIU7gcggnIIr698C/D+LwXZyQ6Lfatp9vIYSbeG/lNuoiTYipGzFY1VeMIFB4yDjNfEX7H3iQ+Cvifr1wshFvq03mmGL/AJabQDnHZVHp3IFfbeheKrHxJpYuLeaGa3kX7ySfMv5H8K9nG1Jqr7mx8/l8HGl73c0PEt9bxhG1LVJ28h94Mt35K7h34IJ/lXj/AMQvib4YvtY+x6ZeafdX+0yrbWsgkkcAjLMwzhV65Jri/wBqL4FtrF19qstWuJ3lnLLbm4IKoEOY1XqzMcgH6V514IsrT9nbw5rEktqy31y4drm4wZniYB0QddoBOCAcHHNelTjKvRTW/ZGOKzKVGtytaW3Ocm+Gtx+0X+2DceE9HuH+w6pJFNrE0LZMFpEqmY84GTgIo7sw681+pHhnTY9L061toI44bWzhS3giQYSGNFCoijsoUAAegFfIv/BK34dQt4F8SePZ7dRqPi7VporeVt2/7HFtwACeFMplPHXA9BX2hpNjuT+7xxXVPfl7Kx8y9Xzd7suW6nap9P8A61TbG/yakii2JineWP8AJqTRHsd9pVvHqc+2FciTOSM1FcYQfxVf1j5dRuOn3s1javqKWFo7syqFBJLGvWfs6UXNrRHz8XOTstTL8beI4/D+kyTkgv0jAPVu3FeA+ML1r3zWkYs7ksx9663xr4qfxFfs3KwJxGPT3+prhNdfLt/d6V8lja88ZLmey2Ptsrw8cJC7+KRxv9rt4dnlj8tp9NnbMsKjLRk9XjB7juB14xzWf8SPAOjfFfwj9k1ILfWMwzbXsTYeJhkAhuquv5joR1Fa+p2XmN8ormr22vvD95JdaeyBpiPPt5RmC4+o67unzDB+vSvHqZe780Nz6aONjJa7nxJ+0P8As6+Mfg/rdxceXdazopO6HUbSMF1U9POReh91GD6CvIrvxjqEVtIryPJHvzu3dxnqPxr9MH8SWeup5MwbTblv+WFwcxuf9hvut9ODXjnxZ/ZE8OePLl7hbU6XfszM9xa4RnyeSezc56+4zXpYXGNWjWjqup5uJw0371OR8ZeA/ixefD3xLa61CHmkgYhkTgyA/wAPPY4+vFd9pP7VML+CtShs72a31CWUzKGHlmMkqQqAHGMgg+grO+JP7IHi/wAM3UzaTMt9ZsTsZHEUsgHqvT9a8V8ReAte0O9MWo6PqEcinkvAx/8AHlGK9iFGjWlzJnkyxdakuRI+k/FP7UY1Hw7otxFeRrceYrFnkJWKRCvzMODznnHBA7Yrgx418SftB/Ee18HeH5o9VutUuVitTv8A3UXy9XbqEQE5JyeOpNeNaX4C1HWLpYbfT7hmY4wYn2j2yR7194/8Ewf2WF8JeLbzVr4KupWdl5sBKcK7OEwuegCM+cc5IrtpxhQ+Dc8upKpWfv7H2n+zn8Hrf4LfCTQPC8E32uPQ7Rbfz8bfPfkvJjtuZmOPQivV7CDZGvb09q4jwz4nXR7uOx1JVhuMbkbrHKvfafUdxXdWs6yIGU/KRx6VzRk2tTOpptsSkKPvc03A/vH8qerbhSfN/s0wUmfRGoeEbea6ZjJcZkCscFe6qfT3ry/4zaQsEsdus9x5Ug+YZHOPwooo4hk1SVu5y5LFPEq55tf+GIdv+sn/AO+h/hXNax4chBb95N1Pcf4UUV49H4T67EfGY0/hS3aT7835j/CqWqeC7R7diWm/Nf8ACiitDlqt3OF8W+BbEoyt5jo3VW2lT+GK4bxJFJ4Le3S1uLiW3mbYYZ28xEHH3T94fgcUUUVorlOqhJl6fwzb6naLJL5jGQAEZGOnuK5nXfBVosf3pipJXDEMMfiKKK83matY7pRVzM034e6eLtf9b1Jxkeg9q9e+A/huGx8dosMk0azQFHA2/MB82OnqAaKK7MPKTerOLHRXIz6G8Z/D6x1PwwyzNO33WGGAKH1BxwfpWX8GfM1rQ7dbiaZ9peLdkZwjFR264FFFep9o8CjrQ17npEPhK3aP/WXH/fQ/wo/4RWD/AJ63H5r/AIUUVsTDY//Z"),
                            new Claim(JwtClaimTypes.Email, "liluyoud@gmail.com"),
                            new Claim("cargo", "Professor"),
                            new Claim("funcao", "Professor Mestre"),
                            new Claim("setor", "EaD"),
                            new Claim("telefone", "(69) 98114-1732"),
                            new Claim("nascimento", "25/07/1973"),
                            new Claim("foto", "data:image/jpeg;base64,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"),
                            new Claim("endereco", @"{ 'street_address': 'Rua João Pedro da Rocha, 2378', 'locality': 'Porto Velho - RO', 'postal_code': '76920-888', 'country': 'Brasil' }", IdentityServer4.IdentityServerConstants.ClaimValueTypes.Json),
                        }).Result;
                        if (!result.Succeeded)
                        {
                            throw new Exception(result.Errors.First().Description);
                        }
                        Console.WriteLine("Liluyoud criado");
                    }
                    else
                    {
                        Console.WriteLine("Liluyoud já cadastrado");
                    }
                }
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }

        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            Console.WriteLine("Clients being populated");
            foreach (var client in Config.GetClients().ToList())
            {
                var registro = context.Clients.SingleOrDefault(c => c.ClientId == client.ClientId);
                if (registro == null)
                {
                    var entity = client.ToEntity();
                    context.Clients.Add(entity);
                }
            }
            context.SaveChanges();

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources being populated");
                foreach (var resource in Config.GetIdentityResources().ToList())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("IdentityResources already populated");
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources being populated");
                foreach (var resource in Config.GetApiResources().ToList())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("ApiResources already populated");
            }
        }
    }
}
